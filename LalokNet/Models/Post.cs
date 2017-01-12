using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LalokNet.Models
{
	public class Post
	{
		public int VkId;
		public int PosterId;
		public string PosterName;
		public string PostContent;
		public DateTime PostDate;
		public int LikesAmount;
		public int RepostsAmount;
		public int CommentsAmount;

		public ICollection<Photo> Photos;
		//public IAttachment[] Attachments;
		
		//public interface IAttachment
		//{
		//}

		public class Photo
		{
			public string Photo604Px;
			//public string Photo130Px;
		}

	}

}