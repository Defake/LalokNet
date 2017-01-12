using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseWorker.Model;
using VK_API;
using VK_API.Models;

namespace DatabaseUpdater
{
	public class VkToDbMapper : VkMapper<User, Post, Group>
	{
		public override User UserConvert(VkApiUser vkUser) => new User()
		{
			FirstName = vkUser.first_name,
			LastName = vkUser.last_name,
			IsHidden = vkUser.hidden != "",
			AvatarLink100 = vkUser.photo_100,
			AvatarLink50 = vkUser.photo_50,
			VkId = vkUser.id,
			Group = new HashSet<Group>(),
			Post = new HashSet<Post>(),
			Friends = new HashSet<User>()
		};

		public override Post PostConvert(VkApiPost vkPost)
		{
			var post = new Post()
			{
				VkId = vkPost.id,
				PostContent = vkPost.text,
				//PosterName = vkPost.name, // Database don't need poster name. 
				PostDate = ConvertDateFromUnix(vkPost.date),
				LikesAmount = vkPost.likes.count,
				CommentsAmount = vkPost.comments.count,
				RepostsAmount = vkPost.reposts.count,

				Photo = new HashSet<Photo>()
			};

			var photos = vkPost.attachments?
							.Where(a => a.type == "photo")?
							.Select(vkPhoto => new Photo {ImageLink = vkPhoto.photo.photo_604})
							.ToArray();

			if (photos != null)
				foreach (var photo in photos)
				{
					photo.Post = post;
					post.Photo.Add(photo);
				}

			return post;
		}

		public override Group GroupConvert(VkApiGroup vkGroup) => new Group()
		{
			StringId = vkGroup.screen_name,
			VkId = vkGroup.id,
			GroupName = vkGroup.name,
			GroupImageLink = vkGroup.photo_200
		};
	}
}
