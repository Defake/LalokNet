using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VK_API.Models;

namespace VK_API
{
	public abstract class VkMapper<TUser, TPost, TGroup>
	{
		public abstract TUser UserConvert(VkApiUser vkUser);
		public abstract TPost PostConvert(VkApiPost vkPost);
		public abstract TGroup GroupConvert(VkApiGroup vkGroup);

		protected static DateTime ConvertDateFromUnix(double timestamp)
		{
			var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return origin.AddSeconds(timestamp);
		}

	}
}
