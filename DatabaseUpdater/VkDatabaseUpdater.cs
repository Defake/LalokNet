using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DatabaseWorker.Controllers;
using DatabaseWorker.Model;

namespace DatabaseUpdater
{
	/// <summary>
	/// Scheduler
	/// </summary>
	public class VkDatabaseUpdater
	{
		private readonly Action<string> _logFunc;
		private static bool _updating = false;

		public VkDatabaseUpdater(Action<string> logFunc)
		{
			_logFunc = logFunc;
		}

		public void Start()
		{
			if (_updating)
				return;

			_updating = true;

			Task.Run(() => { UpdateLoop(); });
		}

		private void UpdateLoop()
		{
			while (_updating)
			{
				List<Group> groups;
				using (var gc = new GroupController())
					groups = gc.GetAllEntities();

				if (!_updating)
					break;

				// Update group's user graphs
				foreach (var group in groups)
				{
					var time1 = GetGroupUserGraph(group);
					_logFunc(
						Math.Abs(time1 + 1f) < Double.Epsilon
						? $"Graph of group {group.StringId} is up to date"
						: $"Graph of group {group.StringId} was gotten in {time1} seconds");

					if (!_updating)
						break;
				}

				if (!_updating)
					break;

				var time2 = GetNews(new DateTime(2016, 12, 31), 20);
				_logFunc($"All posts were gotten in {time2} seconds");

				if (!_updating)
					break;

				_logFunc("Update iteration ended. Wait for the next scheduled update");
				Thread.Sleep(60_000_000);
			}

			_logFunc("Database updating was aborted");
		}

		public void Stop()
		{
			_updating = false;
		}


		public float GetGroupUserGraph(Group group)
		{
			var timer = new Timer();
			using (var disposable = timer.Start())
			{
				var client = new VkClient(new VkToDbMapper());
				var members = client.GetMembers(group.StringId);

				if (members == null)
					throw new NullReferenceException("Got 0 group members!");

				using (var gc = new GroupController())
				{
					if (members.Length <= gc.GetAllGroupMembers(group.StringId).Count)
						return -1;
				}

				foreach (var member in members)
				{
					using (var gc = new GroupController())
					{
						var gr = gc.GetGroupByStringId(group.StringId);
						member.Group.Add(gr);
						gr.User.Add(member);
					}	
				}

				if (!_updating)
				{
					disposable.Dispose();
					return timer.TimeInMiliseconds / 1000f;
				}

				var friendsDicts = members.ToDictionary(m => m.VkId, m => Task.Run(() => client.GetFriends(m.VkId)));

				using (var controller = new UserController())
				{
					foreach (var user in members)
					{
						user.Friends = new HashSet<User>(friendsDicts[user.VkId].Result);

						controller.AddOrUpdateEntity(user);
						_logFunc($"User {user.FirstName} {user.LastName} was added/updated");

						if (!_updating)
						{
							disposable.Dispose();
							return timer.TimeInMiliseconds / 1000f;
						}
					}
				}
			}

			return timer.TimeInMiliseconds / 1000f;
		}

		public float GetNews(DateTime fromDate, int postsPerUserAmount)
		{
			var vk = new VkClient(new VkToDbMapper());
			var timer = new Timer();
			using (var disposable = timer.Start())
			{
				using (var uc = new UserController())
				{
					var postTasks = uc.GetAllEntities()
						.Where(u => u.IsHidden == false)
						.Select(usr => new
							{
								task = Task.Run(() =>
									vk.GetPosts(usr.VkId, fromDate, postsPerUserAmount)),
								user = usr
							}
						);

					if (!_updating)
					{
						disposable.Dispose();
						return timer.TimeInMiliseconds / 1000f;
					}

					foreach (var postsAnon in postTasks)
					{
						postsAnon.task.Result.ForEach(p => p.User = postsAnon.user);
						Task.Run(() =>
						{
							using (var pc = new PostController())
								postsAnon.task.Result.ForEach(p => pc?.AddOrUpdateEntity(p));

							_logFunc($"{postsPerUserAmount} posts of user {postsAnon.user.FirstName} {postsAnon.user.LastName} have written");
						});

						if (!_updating)
							break;
					}
				}
			}

			//news.Sort(Comparer<Post>.Create((post1, post2) => post2.LikesAmount.count - post1.LikesAmount.count));
			//db.AddUserNews(userId, news);

			return timer.TimeInMiliseconds / 1000f;
		}
	}
}