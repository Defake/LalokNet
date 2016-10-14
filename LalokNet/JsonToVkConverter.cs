using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using LalokNet.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LalokNet 
{
	public class JsonToVkConverter
	{

		public static T Deserialize<T>(string jsonObject, T model, params string[] pathNodes)
		{
			//var a = new
			//{
			//	error = ""
			//};

			//var obj = JsonConvert.DeserializeAnonymousType(jsonObject, a);
			//if (VkInteraction.AnonCast(a, obj).error != null)
			//{
			//	var g = 4;
			//	g++;
			//}
			ChangeJsonObject(ref jsonObject, pathNodes);
			return JsonConvert.DeserializeAnonymousType(jsonObject, model);
		}

		/// <summary>
		/// Changes jsonObject to internal object located at specified path
		/// </summary>
		/// <param name="jsonObject">jsonObject - some kind of rootDirectory. Will change after method ends</param>
		/// <param name="pathNodes">Names of path nodes (or directories) in right order</param>
		/// <exception cref="ArgumentNullException">Throws exception if meets unexisted object name</exception>
		private static void ChangeJsonObject(ref string jsonObject, params string[] pathNodes)
		{
			if (pathNodes == null) return;

			foreach (string pathNode in pathNodes)
			{
				string nextJsonObject = "";
				try
				{
					nextJsonObject = JObject.Parse(jsonObject).SelectToken(pathNode).ToString();
				}
				catch (Exception e)
				{
					Console.WriteLine("");
				}
				if (nextJsonObject != "")
					jsonObject = nextJsonObject;
				else
					throw new ArgumentNullException("There's no object with name " + pathNode);
			}
		}
	}

}