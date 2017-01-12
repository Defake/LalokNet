using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DatabaseWorker.Model;

namespace DatabaseWorker.Controllers
{
	public class GroupController : EntityController<Group>
	{
		public GroupController() : base()
		{
			EntitySet = Db.GroupSet;
		}

		public Group GetGroupByStringId(string strId) 
			=> Db.GroupSet.FirstOrDefault(g => g.StringId != null && g.StringId == strId);


		public ICollection<User> GetAllGroupMembers(string groupStrId) 
			=> GetGroupByStringId(groupStrId).User.ToList();
	}
}
