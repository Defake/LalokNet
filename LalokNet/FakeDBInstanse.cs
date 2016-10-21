using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LalokNet.Models;

namespace LalokNet 
{
	public class FakeDBInstanse
	{
		public static FakeDBInstanse instanse { get; } = new FakeDBInstanse();

		public Dictionary<int, VkUser> users;

		private FakeDBInstanse() { }

	}
}