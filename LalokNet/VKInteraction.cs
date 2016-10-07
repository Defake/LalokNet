using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace LalokNet {

	// ReSharper disable once InconsistentNaming
	public class VKInteraction {

		private int appID;
		private string _protectedKey;
		private readonly string _version;

		public VKInteraction(int appID) {
			appID = 5648132;
			_version = "5.56";
		}

		public VKInteraction(int appID, string key) : this(appID) {
			_protectedKey = "ADvbqKGGpfAfGvBUjcMO";
		}

		/// <summary>
		/// Get all group members
		/// </summary>
		/// <param name="groupID">number or text identifier of the group as string</param>
		/// <param name="count">count of members to get. Default: 200</param>
		/// <param name="fields">Additional fields for users. Default: </param>
		/// <param name="sort">Possible options: id_asc, id_desc, time_asc (default), time_desc </param>
		/// <returns>json array with all group members</returns>
		public string GetMembers(string groupID, int count = 200, params string[] fields)
		{
			string strFields = "";
			if (fields != null)
				//strFields = fields.Aggregate(strFields, (current, field) => current + (current + ","));
				foreach (string field in fields)
					strFields += field + ",";

			return PerformRequest("groups.getMembers", new VKStringParameter("group_id", groupID),
														new VKIntegerParameter("count", count),
														new VKStringParameter("fields", strFields));
		}

		/// <summary>
		/// Get friends array of some user
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="count"></param>
		/// <param name="fields"></param>
		/// <returns></returns>
		public string GetFriends(int userID, int count = 200, params string[] fields) {
			string strFields = "";
			if (fields != null)
				foreach (string field in fields)
					strFields += field + ",";

			return PerformRequest("friends.get", new VKIntegerParameter("user_id", userID),
													new VKIntegerParameter("count", count),
													new VKStringParameter("fields", strFields));
		}

		public string GetPosts(int ownerID, int count = 200) {
			return PerformRequest("wall.get", new VKIntegerParameter("owner_id", ownerID),
													new VKIntegerParameter("count", count));
		}

		/// <summary>
		/// Get User data
		/// </summary>
		/// <param name="userID">string or integer user id</param>
		/// <returns>JSON object with </returns>
		public string GetUser(string userID) {
			return PerformRequest("users.get", new VKStringParameter("user_ids", userID),
											new VKStringParameter("fields", "photo_50,city,verified"),
											new VKStringParameter("name_case", "Nom"));
		}

		/// <summary>
		/// Performs vkapi request
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="args"></param>
		/// <returns>JSON response</returns>
		private string PerformRequest(string methodName, params IVKParameter[] args) {
			string request = "https://api.vk.com/method/" + methodName + "?";

			foreach (IVKParameter p in args) {
				request += p.paramName + "=" + p.GetValue() + "&";
			}

			request += "v=" + _version;

			// Do request
			WebRequest reqGET = WebRequest.Create(request);
			WebResponse webResp = reqGET.GetResponse();
			Stream stream = webResp.GetResponseStream();
			StreamReader sr = new StreamReader(stream);

			return sr.ReadToEnd();
		}

		private abstract class IVKParameter {
			public string paramName { get; protected set; }
			public abstract string GetValue();
		}

		private class VKStringParameter : IVKParameter {
			private readonly string _value;

			public VKStringParameter(string paramName, string value) {
				this.paramName = paramName;
				this._value = value;
			}

			public override string GetValue() {
				return _value;
			}

		}

		private class VKIntegerParameter : IVKParameter {
			private readonly int _value;

			public VKIntegerParameter(string paramName, string value) {
				this.paramName = paramName;
				_value = int.Parse(value);
			}

			public VKIntegerParameter(string paramName, int value) {
				this.paramName = paramName;
				_value = value;
			}

			public override string GetValue() {
				return _value.ToString();
			}

		}



	}

}
