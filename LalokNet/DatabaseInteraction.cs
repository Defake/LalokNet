using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LalokNet.Models;

namespace LalokNet 
{
	public class DatabaseInteraction
	{

		private readonly FakeDBInstanse _database;

		public DatabaseInteraction(FakeDBInstanse database)
		{
			_database = database;
		}
		
		public void WriteUsers(Dictionary<int, VkUser> users) => 
			_database.users = users;

		public IEnumerable<KeyValuePair<int, VkUser>> GetUsers() => 
			_database.users.AsEnumerable();

		/// <summary>
		/// Method REPLACES all the user's news
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="news"></param>
		public void AddUserNews(int userId, List<VkPost> news)
		{
			_database.users[userId].news = news;
		}
	}
}