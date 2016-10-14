using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LalokNet.Models;
using static LalokNet.JsonToVkConverter;


namespace LalokNet {

	public class VkInteraction {

		private int appID;
		private string _protectedKey;
		private readonly string _version;

		public VkInteraction() {
			appID = 5648132;
			_version = "5.56";
			_protectedKey = "ADvbqKGGpfAfGvBUjcMO";
		}

		/// <summary>
		/// Get all group members
		/// </summary>
		/// <param name="groupID">number or text identifier of the group as string</param>
		/// <param name="count">count of members to get. Maximum (and default): 1000</param>
		/// <param name="fields">Additional fields for users. Default: none</param>
		/// <returns>List of VKUser</returns>
		public async Task<VkUser[]> GetMembers(string groupID, int count = 1000, params string[] fields)
		{
			string strFields = MakeSingleParameter(fields);

			string jsonResponse = await Task.Run(() => PerformRequest(
														"groups.getMembers", 
														new VkStringParameter("group_id", groupID),
														new VkIntegerParameter("count", count),
														new VkStringParameter("fields", strFields)));

			return Deserialize(jsonResponse, new VkUser[] { }, "response", "items");
		}

		/// <summary>
		/// Get friends array of some user
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="count">Count of friends to get. Default: all friends</param>
		/// <param name="fields">additional fields to get</param>
		/// <returns></returns>
		public async Task<VkUser[]> GetFriends(int userID, int count = 0, params string[] fields) 
		{
			string strFields = MakeSingleParameter(fields);

			var jsonResponse = await Task.Run(() => PerformRequest(
													"friends.get", 
													new VkIntegerParameter("user_id", userID),
													new VkIntegerParameter("count", count),
													new VkStringParameter("fields", strFields)));

			return Deserialize(jsonResponse, new VkUser[] {}, "response", "items");
		}

		/// <summary>
		/// Take posts from a user's wall
		/// </summary>
		/// <param name="ownerID">User of the wall</param>
		/// <param name="count">Count of posts to get. Maximum (and default) value = 100</param>
		/// <param name="filter">all / owner / others. Default: all</param>
		/// <returns></returns>
		public async Task<VkPost[]> GetPosts(int ownerID, int count = 0, string filter = "all")
		{
			var jsonResponse = await Task.Run(() => PerformRequest(
														"wall.get", 
														new VkIntegerParameter("owner_id", ownerID),
														//new VkIntegerParameter("extended", 1),
														new VkIntegerParameter("count", count),
														new VkStringParameter("filter", filter)
														));

			return Deserialize(jsonResponse, new VkPost[] {}, "response", "items");
		}

		public async Task<int> GetPostsNumber(int ownerID, string filter="all")
		{
			var jsonResponse = await Task.Run(() => PerformRequest(
														"wall.get", 
														new VkIntegerParameter("owner_id", ownerID),
														new VkIntegerParameter("count", 1),
														new VkStringParameter("filter", filter)
														));

			return Deserialize(jsonResponse, new int {}, "response", "count");
		}

		/// <summary>
		/// Get User data
		/// </summary>
		/// <param name="userID">Not null string array of user ids</param>
		/// <param name="fields">Additional fields to get</param>
		/// <returns>JSON object with users data</returns>
		public async Task<VkUser[]> GetUsers(string[] userID, params string[] fields)
		{
			if (userID == null) throw new ArgumentNullException("parameter " + nameof(userID) + "Can't be null array");

			var userIds = MakeSingleParameter(userID);
			var fieldsStr = MakeSingleParameter(fields);

			var jsonResponse =  await Task.Run(() => PerformRequest("users.get", 
														new VkStringParameter("user_ids", userIds),
														new VkStringParameter("fields", fieldsStr),
														new VkStringParameter("name_case", "Nom")));

			return Deserialize(jsonResponse, new VkUser[] {}, "response");
		}

		public static T AnonCast<T>(T typeHolder, object x) {
			return (T) x;
		}

		private static string MakeSingleParameter(string[] parameters)
		{
			if (parameters == null)
				return "";

			string str = "";
			
			foreach (string field in parameters)
				str += field + ",";

			return str.TrimEnd(',');
		}

		/// <summary>
		/// Performs vk api request
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="args"></param>
		/// <returns>JSON object response as a string</returns>
		private string PerformRequest(string methodName, params VkParameter[] args) {
			string request = "https://api.vk.com/method/" + methodName + "?";

			foreach (VkParameter p in args) {
				request += p.paramName + "=" + p.GetValue() + "&";
			}

			request += "v=" + _version;

			// Perform the req
			WebRequest reqGET = WebRequest.Create(request);
			WebResponse webResp = reqGET.GetResponse();
			Stream stream = webResp.GetResponseStream();
			StreamReader sr = new StreamReader(stream);

			return sr.ReadToEnd();
		}

		//static DateTime ConvertDateFromUnix(double timestamp) {
		//	DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
		//	return origin.AddSeconds(timestamp);
		//}

		private abstract class VkParameter {
			public string paramName { get; protected set; }
			public abstract string GetValue();
		}

		private class VkStringParameter : VkParameter {
			private readonly string _value;

			public VkStringParameter(string paramName, string value) {
				this.paramName = paramName;
				this._value = value;
			}

			public override string GetValue() {
				return _value;
			}

		}

		private class VkIntegerParameter : VkParameter {
			private readonly int _value;

			public VkIntegerParameter(string paramName, string value) {
				this.paramName = paramName;
				_value = int.Parse(value);
			}

			public VkIntegerParameter(string paramName, int value) {
				this.paramName = paramName;
				_value = value;
			}

			public override string GetValue() {
				return _value.ToString();
			}

		}
	
	}

}
