using System;

namespace VK_API.Models
{
	public class VkApiPost
	{
		public int id;
		public int from_id;
		//public string name;
		public string text;
		public double date;
		public CountedParameter likes;
		public CountedParameter reposts;
		public CountedParameter comments;
		public VkAttachment[] attachments;

		public class CountedParameter
		{
			public int count;
		}

		public class VkAttachment
		{
			public string type;
			public VkPhoto photo = null;
		}

		public class VkPhoto
		{
			public string photo_604;
			public string photo_130;
		}

	}

}