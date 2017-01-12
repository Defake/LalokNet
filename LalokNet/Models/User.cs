using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LalokNet.Models {

	public class User
	{
		public int VkId;
		public string FirstName;
		public string LastName;
		public string Avatar100PxLink;
		public string Avatar50PxLink;

		public int LikesAverage;
		public int SharesAverage;
		public int CommentsAverage;

		public List<int> FriendsIds;
		public List<Post> UserNews;
	}
}