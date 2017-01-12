using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using VK_API.Models;

namespace VK_API {

	public class VkApiInteraction {

		//private int appID;
		//private string _protectedKey;
		private readonly string _version;

		#region Field Enumerators

		public enum SortMode
		{
			[Description("id_asc")]
			AscendindById,
			[Description("id_desc")]
			DescendingById,
			[Description("time_asc")]
			AscendingByJoinDate,
			[Description("time_desc")]
			DescendingByJoinDate 
		}

		public enum PostsFilter
		{
			[Description("all")]
			AllPosts,
			[Description("owner")]
			OwnerPosts,
			[Description("others")]
			OtherUsersPosts
		}

		public enum UserField
		{
			[Description("sex")]
			Sex,
			[Description("bdate")]
			BirthDate,
			[Description("city")]
			City,
			[Description("country")]
			Country,
			[Description("photo_50")]
			Avatar50Pixels,
			[Description("photo_100")]
			Avatar100Pixels,
			[Description("photo_200_orig")]
			Avatar200PixelsOriginal,
			[Description("photo_200")]
			Avatar200Pixels,
			[Description("photo_400_orig")]
			Avatar400PixelsOriginal,
			[Description("photo_max")]
			Avatar400Pixels,
			[Description("photo_max_orig")]
			AvatarMaxSizeOriginal,
			[Description("online")]
			Online,
			[Description("online_mobile")]
			MobileOnline,
			[Description("lists")]
			Lists,
			[Description("domain")]
			Domain,
			[Description("has_mobile")]
			HasMobile,
			[Description("contacts")]
			Contacts,
			[Description("connections")]
			Connections,
			[Description("site")]
			Site,
			[Description("education")]
			Education,
			[Description("universities")]
			Universities,
			[Description("schools")]
			Schools,
			[Description("can_post")]
			CanPost,
			[Description("can_see_all_posts")]
			CanSeeAllPosts,
			[Description("can_see_audio")]
			CanSeeAudio,
			[Description("can_write_private_message")]
			CanWritePrivateMessage,
			[Description("status")]
			Status,
			[Description("last_seen")]
			LastSeen,
			[Description("common_count")]
			CommonCount,
			[Description("relation")]
			Relation,
			[Description("relatives")]
			Relatives,
			[Description("first_name")]
			FirstName,
			[Description("last_name")]
			LastName
		}

		//public enum FriendsField
		//{
		//	[Description("first_name")]
		//	FirstName,
		//	[Description("last_name")]
		//	LastName,
		//	[Description("photo_100")]
		//	Avatar100Pixels,
		//	[Description("photo_50")]
		//	Avatar50Pixels
		//}

		#endregion

		public VkApiInteraction()
		{
			// magic strings
			//appID = 5648132;
			_version = "5.56";
		}

		public VkApiGroup GetGroupInfo(string groupId)
		{
			var jsonResponse = PerformRequest(
						"groups.getById",
						new VkParameter("group_ids", groupId)
						);

			return JsonToVkConverter.Deserialize(jsonResponse, new VkApiGroup[] {}, "response")[0];
		}

		/// <summary>
		/// Get all group members
		/// </summary>
		/// <param name="groupId">number or text identifier of the group as string</param>
		/// <param name="count">count of members to get. Maximum: 1000</param>
		/// <param name="fields">Additional fields for users. Default: none</param>
		/// <param name="offset"></param>
		/// <param name="sort"></param>
		/// <returns>Array of VkApiUsers from this group</returns>
		public VkApiUser[] GetGroupMembers(string groupId, int count = 1000, int offset = 0, SortMode sort = SortMode.AscendindById, params UserField[] fields) 
		{
			var jsonResponse = PerformRequest(
						"groups.getMembers",
						new VkParameter("group_id", groupId),
						new VkParameter("count", count.ToString()),
						new VkParameter("offset", offset.ToString()),
						new VkParameter("sort", sort.Description()),
						MakeFieldsVkParameter(fields.AsEnumerable())
						);

			return JsonToVkConverter.Deserialize(jsonResponse, new VkApiUser[] { }, "response", "items");
		}

		/// <summary>
		/// Get friends array of some user
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="count">Count of friends to get. Default: all friends</param>
		/// <param name="offset"></param>
		/// <param name="fields">additional fields to get</param>
		/// <returns>Array of friends (VkApiUsers)</returns>
		public VkApiUser[] GetFriends(int userId, int count, int offset, params UserField[] fields) {
			var jsonResponse = PerformRequest(
									"friends.get",
									new VkParameter("user_id", userId.ToString()),
									new VkParameter("count", count.ToString()),
									new VkParameter("offset", offset.ToString()),
									new VkParameter("name_case", "Nom"),
									MakeFieldsVkParameter(fields.AsEnumerable())
									);

			return JsonToVkConverter.Deserialize(jsonResponse, new VkApiUser[] { }, "response", "items");
		}

		/// <summary>
		/// Take posts from a user's wall
		/// </summary>
		/// <param name="ownerId">User of the wall</param>
		/// <param name="amount">this out parameter will return amount of available posts</param>
		/// <param name="count">Count of posts to get. Maximum value = 100</param>
		/// <param name="offset">From what index of post to start</param>
		/// <param name="filter">Which posts to get. Default: all</param>
		/// <returns>Array of VkApiPosts from the wall of a person</returns>
		public VkApiPost[] GetPosts(int ownerId, out int amount, int count = 100, int offset = 0, PostsFilter filter = PostsFilter.AllPosts)
		{
			var jsonResponse = PerformRequest(
									"wall.get",
									new VkParameter("owner_id", ownerId.ToString()),
									new VkParameter("count", count.ToString()),
									new VkParameter("filter", filter.Description()),
									new VkParameter("offset", offset.ToString())
									);

			amount = JsonToVkConverter.Deserialize(jsonResponse, new int(), "response", "count");

			//throw new Exception("POST AMOUNT EQUALS " + amount);

			return JsonToVkConverter.Deserialize(jsonResponse, new VkApiPost[] { }, "response", "items");
		}

		///// <summary>
		///// Get User data
		///// </summary>
		///// <param name="userId">Not null string array of user ids</param>
		///// <param name="fields">Additional fields to get</param>
		///// <returns>JSON object with users data</returns>
		//public string GetUsers(string[] userId, params UserField[] fields) {
		//	if (userId == null)
		//		throw new ArgumentNullException("parameter " + nameof(userId) + "Can't be null");
			
		//	var userIds = userId.Aggregate("", (current, id) => current + (id + ",")).TrimEnd(',');

		//	return PerformRequest("users.get",
		//							new VkParameter("user_ids", userIds),
		//							new VkParameter("name_case", "Nom"),
		//							MakeFieldsVkParameter(fields)
		//							);

		//	//return Deserialize(jsonResponse, new VkApiUser[] { }, "response");
		//	//return jsonResponse;
		//}

		//public static T AnonCast<T>(T typeHolder, object x) {
		//	return (T) x;
		//}

		/// <summary>
		/// Make VkParameter "fields" from array of UserFields enum values
		/// </summary>
		/// <param name="parameters">Array of enum values</param>
		/// <returns>VKParameter. Name "fields" value - string values of enum-values from the array</returns>
		private static VkParameter MakeFieldsVkParameter(IEnumerable<UserField> parameters)
		{
			return new VkParameter(
				"fields",
				parameters.Any()
					? parameters
						.Aggregate("", (current, p) => current + p.Description() + ",")
						.TrimEnd(',')
					: ""
				);
		}

		/// <summary>
		/// Performs vk api request
		/// </summary>
		/// <param name="methodName"></param>
		/// <param name="args"></param>
		/// <returns>JSON object response as a string</returns>
		private string PerformRequest(string methodName, params VkParameter[] args) {
			// Form request
			string requestUrl = "https://api.vk.com/method/" + methodName + "?";

			requestUrl = args.Aggregate(requestUrl, (current, p) => current + p.ToString() + "&");

			requestUrl += "v=" + _version;

			// Perform the request
			var httpRequest = WebRequest.CreateHttp(requestUrl);
			string result;
			using (var httpResponse = (HttpWebResponse) httpRequest.GetResponse())
			{
				using (var sr = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
				{
					result = sr.ReadToEnd();
				}
			}
			return result;
		}

		private class VkParameter
		{
			private string ParamName { get; }
			private string Value { get; }

			public VkParameter(string paramName, string value)
			{
				ParamName = paramName;
				Value = value;
			}

			public override string ToString()
			{
				return new StringBuilder()
					.Append(ParamName)
					.Append('=')
					.Append(Value)
					.ToString();
			}
		}

	}

	public static class EnumExtensions {

		/// <summary>
		/// Gets the value from a DescriptionAttribute applied to the enum
		/// </summary>
		/// <param name="value">Enum value</param>
		/// <returns>DescriptionAttribute value, or null if no attribute is applied</returns>
		public static string Description(this Enum value) {
			var descriptionAttributes =
				value.GetType()
				//.GetField(value.ToString(CultureInfo.InvariantCulture))
				.GetField(value.ToString())
				.GetCustomAttributes(typeof(DescriptionAttribute), false);

			return descriptionAttributes.Length > 0
				? ((DescriptionAttribute)descriptionAttributes[0]).Description
				: null;
		}

	}

}
