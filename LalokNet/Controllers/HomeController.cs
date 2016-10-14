using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LalokNet.Models;
using Newtonsoft.Json;

namespace LalokNet.Controllers
{

	public class HomeController : Controller
	{

		/*
		 * версия метода с доверянием асинка фреймворку
		 // GET: Home
		public async Task<ActionResult> Index()
        {
			VkInteraction vk = new VkInteraction();

	        Stopwatch watch = new Stopwatch();
			watch.Start();
	        long time = watch.ElapsedMilliseconds;

	        VkUser[] members = Task.Run(() => vk.GetMembers("csu_iit", fields: "true")).Result;

			//var friendsTask = members.ToDictionary(u => u.id, u => vk.GetFriends(u.id));

			foreach (VkUser u in members)
			{
				if (u.deactivated != "")
				{
					u.friends = new VkUser[0];
					continue;
				}
					
				//u.friends = Task.Run(() => vk.GetFriends(u.id)).Result;
				u.friends = await vk.GetFriends(u.id);
			}

			VkDataHolder.instance.users = members.ToDictionary(user => user.id, user => user);

			time = (watch.ElapsedMilliseconds - time) / 1000;
			watch.Stop();
	        ViewBag.ExecTime = time;

			return View(VkDataHolder.instance.users);
        }
			 */

		// GET: Home
		public async Task<ActionResult> Index()
        {
			VkInteraction vk = new VkInteraction();

	        Stopwatch watch = new Stopwatch();
			watch.Start();
	        long time = watch.ElapsedMilliseconds;

	        VkUser[] members = Task.Run(() => vk.GetMembers("csu_iit", fields: "photo_100, photo_50")).Result;

			var friendsTask = members
				.Where(u => u.deactivated == "")
				.ToDictionary(u => u.id, u => vk.GetFriends(u.id, 0, "first_name", "last_name", "photo_100", "photo_50"));

			foreach (VkUser u in members)
				if (u.deactivated != "")
					u.friends = new VkUser[0];
				else
					u.friends = await friendsTask[u.id];

			VkDataHolder.instance.users = members.ToDictionary(user => user.id, user => user);

			time = (watch.ElapsedMilliseconds - time) / 1000;
			watch.Stop();
	        ViewBag.ExecTime = time;

			return View(VkDataHolder.instance.users);
        }

		public async Task<ActionResult> GetUserNews(int userId)
		{
			if (userId == 0) throw new ArgumentOutOfRangeException(nameof(userId));
			VkInteraction vk = new VkInteraction();
			List<VkPost> posts = new List<VkPost>();

			Stopwatch watch = new Stopwatch();
			watch.Start();
			long time = watch.ElapsedMilliseconds;

			var postsTask = VkDataHolder.instance.users[userId].friends
				.Where(u => u.deactivated == "" && u.hidden == "")
				.ToDictionary(u => u.id, u => vk.GetPosts(u.id, filter: "owner"));

			foreach (VkUser friend in VkDataHolder.instance.users[userId].friends)
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
				
				posts.AddRange(friendPosts);
			}

			time = (watch.ElapsedMilliseconds - time) / 1000;
			watch.Stop();
			ViewBag.ExecTime = time;

			posts.Sort(Comparer<VkPost>.Create((post1, post2) => post2.likes.count - post1.likes.count));

			return View(posts);
		}

		/*public async Task<ActionResult> GetUserNews(int userId)
		{
			if (userId == 0) throw new ArgumentOutOfRangeException(nameof(userId));
			VkInteraction vk = new VkInteraction();
			List<VkPost> posts = new List<VkPost>();

			Stopwatch watch = new Stopwatch();
			watch.Start();
			long time = watch.ElapsedMilliseconds;
			
			foreach (VkUser friend in VkDataHolder.instance.users[userId].friends)
			{
				// If this inactive user, pass this iteration
				if (friend.hidden != "" || friend.deactivated != "")
					continue;
				
				VkPost[] friendPosts = vk.GetPosts(friend.id, count: 5, filter: "owner").Result;

				if (friendPosts == null || friendPosts.Length == 0)
					continue;

				//string name = vk.GetUsers(new [] {friendPosts[0].from_id.ToString()}, fields: "true")[0].first_name;
				string name = friend.first_name + " " + friend.last_name;
				foreach (VkPost post in friendPosts)
					post.name = name;
				
				posts.AddRange(friendPosts);
			}

			time = (watch.ElapsedMilliseconds - time) / 1000;
			watch.Stop();
			ViewBag.ExecTime = time;

			return View(posts);
		}*/

	}
}