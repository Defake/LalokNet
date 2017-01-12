using System.Collections.Generic;
using System.Linq;
using DatabaseWorker.Controllers;
using LalokNet.Models;

namespace LalokNet.AppClasses 
{
	public class VkApp
	{
		public static List<Group> GetIndexPage()
		{
			using (var gc = new GroupController())
			{
				var mapper = new DbToModelMapper();
				return gc.GetAllEntities().Select(g => mapper.GroupConvert(g)).ToList();
			}
		}

		public static List<User> GetGroupPage(string groupStrId)
		{
			using (var gc = new GroupController())
			{
				var mapper = new DbToModelMapper();
				return gc.GetAllGroupMembers(groupStrId).Select(m => mapper.UserConvert(m)).ToList();
			}
		}

		public static List<Post> GetUserPage(int userVkId, int sortMethod)
		{
			using (var pc = new PostController())
			using (var uc = new UserController())
			{
				var mapper = new DbToModelMapper();

				var friendsIds = mapper.UserConvert(uc.GetEntityById(userVkId)).FriendsIds;

				var posts = friendsIds
					.Select(pc.GetAllUserPosts)
					.SelectMany(x => x)
					.Select(mapper.PostConvert)
					.ToList();

				var friends = friendsIds.Select(uc.GetEntityById).Select(mapper.UserConvert).ToList();
				foreach (var friend in friends)
				{
					var likes = 0;
					var shares = 0;
					var comments = 0;
					var amount = 0;
					pc.GetAllUserPosts(friend.VkId).Select(mapper.PostConvert).ToList().ForEach((p) =>
					{
						likes += p.LikesAmount;
						shares += p.RepostsAmount;
						comments += p.CommentsAmount;
						amount++;
					});

					if (amount == 0)
						amount = 1;

					friend.LikesAverage = likes / amount;
					friend.SharesAverage = shares / amount;
					friend.CommentsAverage = comments / amount;
					if (friend.LikesAverage == 0)
						friend.LikesAverage = 1;
					if (friend.SharesAverage == 0)
						friend.SharesAverage = 1;
					if (friend.CommentsAverage == 0)
						friend.CommentsAverage = 1;
				}

				switch (sortMethod)
				{
					case 0:
						posts.Sort((p1, p2) => 
							p2.LikesAmount / friends.FirstOrDefault(f => f.VkId == p2.PosterId).LikesAverage 
							- p1.LikesAmount / friends.FirstOrDefault(f => f.VkId == p1.PosterId).LikesAverage);
						break;
					case 1:
						posts.Sort((p1, p2) => 
						p2.RepostsAmount / friends.FirstOrDefault(f => f.VkId == p2.PosterId).SharesAverage
						- p1.RepostsAmount / friends.FirstOrDefault(f => f.VkId == p1.PosterId).SharesAverage);
						break;
					case 2:
						posts.Sort((p1, p2) => 
						p2.CommentsAmount / friends.FirstOrDefault(f => f.VkId == p2.PosterId).CommentsAverage
						- p1.CommentsAmount / friends.FirstOrDefault(f => f.VkId == p1.PosterId).CommentsAverage);
						break;
					default:
						break;
				}

				
				
				return posts;
			}
		}


	}
}