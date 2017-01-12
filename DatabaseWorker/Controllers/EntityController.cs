using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseWorker.Model;

namespace DatabaseWorker.Controllers
{
	public abstract class EntityController<TEntity> : IDisposable 
		where TEntity : EntityBase
	{
		protected readonly ModelVkContainer Db;
		protected DbSet<TEntity> EntitySet;

		protected EntityController()
		{
			Db = new ModelVkContainer();
		}

		public List<TEntity> GetAllEntities() 
			=> EntitySet.ToList();

		public TEntity GetEntityById(int id) 
			=> EntitySet.FirstOrDefault(entity => entity.VkId == id);

		/// <summary>
		/// Add entity with id = 0 or update an entity when Id != 0
		/// </summary>
		/// <param name="entity">Entity to add or update</param>
		/// <returns></returns>
		public virtual TEntity AddOrUpdateEntity(TEntity entity)
		{
			//Db.Configuration.AutoDetectChangesEnabled = false;
			var obj = EntitySet.Find(entity.VkId);
			if (obj == null)
			{
				//EntitySet.Add(entity);
				Db.Entry(entity).State = EntityState.Added;
				
			}
			else
				Db.Entry(obj).CurrentValues.SetValues(entity);

			Db.SaveChanges();
			//Db.ChangeTracker.DetectChanges();
			return entity;
		}

		public void Delete(int id)
		{
			Db.Entry(GetEntityById(id)).State = EntityState.Deleted;
			Db.SaveChanges();
		}

		public void Dispose()
		{
			Db.Dispose();
		}

	}
	
}
