using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LalokNet.Models;
using Newtonsoft.Json;

namespace LalokNet.Controllers
{

	public class HomeController : Controller
	{
		
        // GET: Home
        public ActionResult Index()
        {
			VKInteraction vk = new VKInteraction(5648132);

	        Stopwatch watch = new Stopwatch();
			watch.Start();
	        long time = watch.ElapsedMilliseconds;

	        var groupMembersObj = new
	        {
		        response = new
		        {
					count = 0,
			        items = new VKUser[] {}
		        }
	        };

	        var jsonMembers = vk.GetMembers("csu_iit", count: 30, fields: "true");
			var members = JsonConvert.DeserializeAnonymousType(jsonMembers, groupMembersObj);
	        VKDataHolder.instance.users = members.response.items.ToDictionary(user => user.id, user => user);

	        var friendsObj = new {
				response = new {
					count = 0,
					items = new int[] {}
				}
			};

			foreach (VKUser u in VKDataHolder.instance.users.Values)
	        {
		        string jsonFriends = vk.GetFriends(u.id);
		        var friends = JsonConvert.DeserializeAnonymousType(jsonFriends, friendsObj);
		        if (friends.response != null) 
					u.friends = friends.response.items;
	        }

	        time = (watch.ElapsedMilliseconds - time) / 1000;
			watch.Stop();
	        ViewBag.ExecTime = time;

			return View(VKDataHolder.instance.users);
        }

		public ActionResult GetUserNews(int userId)
		{
			if (userId == 0) throw new ArgumentOutOfRangeException(nameof(userId));
			VKInteraction vk = new VKInteraction(5648132);
			List<string> posts = new List<string>();

			Stopwatch watch = new Stopwatch();
			watch.Start();
			long time = watch.ElapsedMilliseconds;

			var postObj = new {
				response = new {
					count = 0,
					items = new [] { new { text = "" } }
				}
			};

			foreach (int friendId in VKDataHolder.instance.users[userId].friends)
			{
				var jsonPosts = vk.GetPosts(friendId, count: 5);
				var friendPosts = JsonConvert.DeserializeAnonymousType(jsonPosts, postObj);

				if (friendPosts.response != null)
					posts.AddRange(friendPosts.response.items.Select(item => item.text));
			}

			time = (watch.ElapsedMilliseconds - time) / 1000;
			watch.Stop();
			ViewBag.ExecTime = time;

			return View(posts);
		}
    }
}