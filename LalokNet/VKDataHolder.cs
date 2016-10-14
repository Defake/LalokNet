using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LalokNet.Models;

namespace LalokNet 
{
	public class VkDataHolder
	{
		public static VkDataHolder instance { get; } = new VkDataHolder();

		public Dictionary<int, VkUser> users;

		public VkDataHolder()
		{
			
		}
	}
}