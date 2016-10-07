using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LalokNet.Models;

namespace LalokNet 
{
	public class VKDataHolder
	{
		public static VKDataHolder instance { get; } = new VKDataHolder();

		public Dictionary<int, VKUser> users;

		public VKDataHolder()
		{

		}
	}
}