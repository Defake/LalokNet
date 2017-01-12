using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DatabaseWorker.Model;

namespace DatabaseWorker.Controllers
{
	public class PostController : EntityController<Post>
	{
		public PostController() : base()
		{
			EntitySet = Db.PostSet;
		}

		public override Post AddOrUpdateEntity(Post entity)
		{
			var dbPost = EntitySet.FirstOrDefault(e => e.VkId == entity.VkId && e.User.VkId == entity.User.VkId);

			// We don't need to overwrite posts, because posts are don't changed
			if (dbPost != null)
				return dbPost;

			Db.Configuration.AutoDetectChangesEnabled = false;

			entity.Id = Db.PostSet.ToArray().LastOrDefault()?.Id ?? 0;

			var user = Db.UserSet.FirstOrDefault(u => u.VkId == entity.User.VkId);
			Db.Entry(user).State = EntityState.Unchanged;
			entity.User = user ?? entity.User;

			var i = 0;
			foreach (var photo in entity.Photo)
				photo.Id = Db.PhotoSet.Count() + i++;

			Db.Entry(entity).State = EntityState.Added;

			bool failed;
			do
			{
				try
				{
					Db.SaveChanges();
					Db.ChangeTracker.DetectChanges();
					Db.Configuration.AutoDetectChangesEnabled = true;
					failed = false;
				}
				catch (Exception)
				{
					entity.Id++;
					failed = true;
				}
			} while (failed);

			return entity;
		}

		public List<Post> GetAllUserPosts(int userVkId) 
			=> Db.UserSet.FirstOrDefault(u => u.VkId == userVkId)?.Post.ToList();
	}
}
