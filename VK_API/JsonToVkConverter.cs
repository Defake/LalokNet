using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VK_API 
{
	public class JsonToVkConverter
	{
		public static T Deserialize<T>(string jsonObject, T model, params string[] pathNodes)
		{
			return 
				!ChangeJsonDirectory(ref jsonObject, pathNodes) 
				? default(T) 
				: JsonConvert.DeserializeAnonymousType(jsonObject, model);
		}

		/// <summary>
		/// Changes jsonObject to internal object located at specified path
		/// </summary>
		/// <param name="jsonObject">jsonObject - some kind of rootDirectory. Will change after method ends</param>
		/// <param name="pathNodes">Names of path nodes (or directories) in right order</param>
		/// <exception cref="ArgumentNullException">Throws exception if meets unexisted object name</exception>
		private static bool ChangeJsonDirectory(ref string jsonObject, params string[] pathNodes)
		{
			if (pathNodes == null) return true;

			foreach (string pathNode in pathNodes)
			{
				string nextJsonObject = "";
				try
				{
					nextJsonObject = JObject.Parse(jsonObject).SelectToken(pathNode).ToString();
				}
				catch (NullReferenceException)
				{
					return false;
				}
				if (nextJsonObject != "")
					jsonObject = nextJsonObject;
				else
					throw new ArgumentNullException("There's no object with name " + pathNode);
			}

			return true;
		}
	}

}