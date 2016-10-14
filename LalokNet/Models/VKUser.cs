using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LalokNet.Models {

	public class VkUser
	{

		public int id;
		public string first_name;
		public string last_name;
		public string photo_100;
		public string photo_50;
		public VkUser[] friends;
		public string hidden = "";
		public string deactivated = "";

	}
}