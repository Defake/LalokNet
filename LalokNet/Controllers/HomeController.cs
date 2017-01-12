using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DatabaseWorker.Controllers;
using LalokNet.AppClasses;
using LalokNet.Models;
using Newtonsoft.Json;

namespace LalokNet.Controllers
{

	public class HomeController : Controller
	{
		// GET: Home
		public ActionResult Index()
		{
			return View(VkApp.GetIndexPage());
		}

		public ActionResult GroupPage(string groupStringId)
		{
			if (groupStringId == "") throw new ArgumentOutOfRangeException(nameof(groupStringId));

			Group group = null;
			using (var gc = new GroupController())
			{
				group = new DbToModelMapper().GroupConvert(gc.GetGroupByStringId(groupStringId));
			}

			ViewBag.Img = group.Image;
			ViewBag.Name = group.Name;

			return View(VkApp.GetGroupPage(groupStringId));
		}

		public ActionResult GetUserPage(int userId)
		{
			if (userId == 0) throw new ArgumentOutOfRangeException(nameof(userId));

			User user = null;
			using (var uc = new UserController())
				user = new DbToModelMapper().UserConvert(uc.GetEntityById(userId));

			ViewBag.User = user;

			return View();
		}

		[HttpPost]
		public ActionResult AddPosts(string data)
		{
			var arr = data.Split(',');
			var userId = Int32.Parse(arr[0]);
			var fromId = Int32.Parse(arr[1]);
			var sortMethod = Int32.Parse(arr[2]);

			var newId = fromId + 10;
			var posts = VkApp.GetUserPage(userId, sortMethod);
			var selectedPosts = new List<Post>();
			for (int i = fromId; i < newId; i++)
				selectedPosts.Add(posts[i]);

			ViewBag.postId = fromId;
			ViewBag.newId = newId;

			return PartialView("PostBlocks", selectedPosts);

		}

	}
}