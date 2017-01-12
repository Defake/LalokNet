using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseWorker.Model;
using VK_API;

namespace DatabaseUpdater
{
	public class VkClient
	{
		private readonly VkApiInteraction _vkApi;
		private readonly VkToDbMapper _mapper;

		public VkClient(VkToDbMapper mapper)
		{
			_mapper = mapper;
			_vkApi = new VkApiInteraction();
		}

		public VkClient(VkApiInteraction vkApi, VkToDbMapper mapper) : this(mapper)
		{
			_vkApi = vkApi;
		}

		public Group GetGroup(string groupId) => 
			_mapper.GroupConvert(_vkApi.GetGroupInfo(groupId));

		public User[] GetMembers(string groupId)
		{
			var vkGroupMembers = _vkApi.GetGroupMembers(groupId, 1000, 0,
				VkApiInteraction.SortMode.AscendindById,
				VkApiInteraction.UserField.Avatar100Pixels,
				VkApiInteraction.UserField.Avatar50Pixels);

			return vkGroupMembers
				.Where(usr => usr.deactivated == "")
				.Select(usr => _mapper.UserConvert(usr))
				.ToArray();
		}

		public User[] GetFriends(int userId)
		{
			var vkFriends = _vkApi.GetFriends(userId, 0, 0,
				VkApiInteraction.UserField.FirstName,
				VkApiInteraction.UserField.LastName,
				VkApiInteraction.UserField.Avatar100Pixels,
				VkApiInteraction.UserField.Avatar50Pixels);

			return vkFriends
				.Where(fnd => fnd.deactivated == "")
				.Select(fnd => _mapper.UserConvert(fnd))
				.ToArray();
		}

		/// <summary>
		/// Gets from VK API certain amount of posts
		/// </summary>
		/// <param name="ownerId">Id of the user which wall posts to get from</param>
		/// <param name="fromDate">Date until get the posts</param>
		/// <param name="maxCount">Amount of posts to get. If 0 - will get all the posts</param>
		/// <returns>Returns enumerated array of Posts</returns>
		public List<Post> GetPosts(int ownerId, DateTime fromDate, int maxCount = 0)
		{
			int? postsAmount = null;
			var postsCount = 0;
			var maxPortionSize = Math.Max(100, maxCount);
			var posts = new List<Post>();

			while ((postsCount < maxCount || maxCount == 0)
				&& (postsCount < postsAmount || postsAmount == null))
			{
				var vkPosts = _vkApi.GetPosts(ownerId, out int amount, Math.Min(maxPortionSize - postsCount, 100), postsCount, VkApiInteraction.PostsFilter.OwnerPosts);
				postsAmount = amount;
				postsCount += vkPosts.Length;

				if (vkPosts.Length == 0)
					break;

				posts.AddRange(vkPosts.Select(t => _mapper.PostConvert(t)));

				if (posts.Last().PostDate < fromDate)
					break;
			}

			return posts;
		}

	}
}