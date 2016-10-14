using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LalokNet.Models
{
	public class VkPost
	{
		public int id;
		public int from_id;
		public string name;
		public string text;
		public double date;
		public CountedParameter likes;
		public CountedParameter reposts;
		public CountedParameter comments;
		public Attachment[] attachments;

		public DateTime getDate => new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(date);

		public class CountedParameter
		{
			public int count;
		}

		public class Attachment
		{
			public string type;
			public Photo photo = null;
		}

		public class Photo
		{
			public string photo_604;
			public string photo_130;
		}

	}

}