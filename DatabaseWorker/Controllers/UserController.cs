using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseWorker.Model;

namespace DatabaseWorker.Controllers
{
	public class UserController : EntityController<User>
	{
		public UserController() : base()
		{
			EntitySet = Db.UserSet;
		}

		public new User AddOrUpdateEntity(User entity)
		{
			Db.Configuration.AutoDetectChangesEnabled = false;
			var dbUser = EntitySet.Find(entity.VkId);

			// Set entity relations with groups in db. Delete other user groups
			var userGroups = entity.Group.Select(g => Db.GroupSet.FirstOrDefault(dbg => dbg.StringId == g.StringId)).ToArray();
			entity.Group = new HashSet<Group>();
			foreach (var g in userGroups)
			{
				entity.Group.Add(g);
				var userGroupEntry = g.User.FirstOrDefault(u => u.VkId == entity.VkId);
				if (userGroupEntry == null)
					g.User.Add(dbUser ?? entity);
			}

			// Set entity relations with friends
			// (Replace existing friends with friends entities from DB)
			var oldFriends = entity.Friends;
			entity.Friends = new HashSet<User>();
			foreach (var friend in oldFriends)
			{
				var dbFriend = Db.UserSet.FirstOrDefault(u => u.VkId == friend.VkId);
				var newFriend = dbFriend ?? friend;
				if (dbFriend != null)
					Db.Entry(dbFriend).State = EntityState.Unchanged;

				var userEntry = newFriend.Friends.FirstOrDefault(u => u.VkId == entity.VkId);
				if (userEntry == null)
					newFriend.Friends.Add(dbUser ?? entity);

				entity.Friends.Add(newFriend);
			}


			if (dbUser == null)
			{
				Db.Entry(entity).State = EntityState.Added;
			}
			else
			{
				Db.Entry(dbUser).CurrentValues.SetValues(entity);
				Db.Entry(dbUser).State = EntityState.Modified;
			}
			Db.SaveChanges();
			Db.ChangeTracker.DetectChanges();
			Db.Configuration.AutoDetectChangesEnabled = true;

			return entity;
		}
		
	}
}
