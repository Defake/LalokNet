using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LalokNet.Models;
using static LalokNet.Models.Post;

namespace LalokNet.AppClasses
{
	public class DbToModelMapper
	{
		public User UserConvert(DatabaseWorker.Model.User dbUser) => new User
		{
			VkId = dbUser.VkId,
			FirstName = dbUser.FirstName,
			LastName = dbUser.LastName,
			Avatar100PxLink = dbUser.AvatarLink100,
			Avatar50PxLink = dbUser.AvatarLink50,
			FriendsIds = dbUser.Friends.Select(fnd => fnd.VkId).ToList(),
			UserNews = dbUser.Post.Select(PostConvert).ToList()
		};
		
		public Post PostConvert(DatabaseWorker.Model.Post dbPost) => new Post()
		{
			VkId = dbPost.VkId,
			PostContent = dbPost.PostContent,
			PosterId = dbPost.User.VkId,
			PosterName = $"{dbPost.User.FirstName} {dbPost.User.LastName}",
			PostDate = dbPost.PostDate,
			LikesAmount = dbPost.LikesAmount,
			CommentsAmount = dbPost.CommentsAmount,
			RepostsAmount = dbPost.RepostsAmount,
			Photos = dbPost.Photo.Select(p => new Photo() {Photo604Px = p.ImageLink}).ToList()
		};

		public Group GroupConvert(DatabaseWorker.Model.Group dbGroup) => new Group()
		{
			VkId = dbGroup.VkId,
			StringId = dbGroup.StringId,
			Image = dbGroup.GroupImageLink,
			Name = dbGroup.GroupName
		};

	}
}