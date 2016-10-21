using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LalokNet.Models;

namespace LalokNet 
{
	public class VkApp
	{

		public static async Task<long> GetUsersGraph()
		{
			VkInteraction vk = new VkInteraction();
			VkUser[] members = null;
			
			Func<Task> lam = async () =>
			{
				members = Task.Run(() => vk.GetMembers("csu_iit", fields: "photo_100, photo_50")).Result;

				var friendsTask = members
					.Where(u => u.deactivated == "")
					.ToDictionary(u => u.id, u => vk.GetFriends(u.id, 0, "first_name", "last_name", "photo_100", "photo_50"));

				foreach (VkUser u in members)
					if (u.deactivated != "")
						u.friends = new VkUser[0];
					else
						u.friends = await friendsTask[u.id];	
			};

			if (members != null)
				new DatabaseInteraction(FakeDBInstanse.instanse).WriteUsers(members.ToDictionary(user => user.id, user => user));
			else
				throw new NullReferenceException("Got 0 group members!");

			return await PerformMethodWithTimeMeasurement(lam);
		}

		public static async Task<long> GetNews(int userId)
		{
			VkInteraction vk = new VkInteraction();
			List<VkPost> news = new List<VkPost>();
			var db = new DatabaseInteraction(FakeDBInstanse.instanse);

			//Action lam = async () =>
			//{
				var postsTask = db.GetUsers().ElementAt(userId).Value.friends
					.Where(u => u.deactivated == "" && u.hidden == "")
					.ToDictionary(u => u.id, u => vk.GetPosts(u.id, filter: "owner"));

				foreach (VkUser friend in db.GetUsers().ElementAt(userId).Value.friends)
				{
					// If this inactive user, pass this iteration
					if (!postsTask.ContainsKey(friend.id))
						continue;

					VkPost[] friendPosts = await postsTask[friend.id];

					if (friendPosts == null || friendPosts.Length == 0)
						continue;

					string name = friend.first_name + " " + friend.last_name;
					foreach (VkPost post in friendPosts)
						post.name = name;

					news.AddRange(friendPosts);
				}
			//};

			news.Sort(Comparer<VkPost>.Create((post1, post2) => post2.likes.count - post1.likes.count));
			db.AddUserNews(userId, news);

			return 0;//PerformMethodWithTimeMeasurement(lam);
		}

		private static async Task<long> PerformMethodWithTimeMeasurement(Func<Task> method, params object[] args)
		{

			Stopwatch watch = new Stopwatch();
			watch.Start();
			long time = watch.ElapsedMilliseconds;

			await method();

			time = (watch.ElapsedMilliseconds - time) / 1000;
			watch.Stop();

			return time;
		}

	}
}