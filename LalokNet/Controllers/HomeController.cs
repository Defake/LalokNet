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

		// GET: Home
		public async Task<ActionResult> Index()
        {
			var db = new DatabaseInteraction(FakeDBInstanse.instanse);

			var task = VkApp.GetUsersGraph();

			ViewBag.ExecTime = await task;

			return View(db.GetUsers());
        }

		public ActionResult GetUserNews(int userId)
		{
			if (userId == 0) throw new ArgumentOutOfRangeException(nameof(userId));

			ViewBag.ExecTime = VkApp.GetNews(userId);

			return View(new DatabaseInteraction(FakeDBInstanse.instanse)
						.GetUsers().ElementAt(userId).Value.news);
		}

	}
}