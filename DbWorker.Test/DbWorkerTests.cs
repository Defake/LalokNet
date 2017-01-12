using System;
using System.Collections.Generic;
using DatabaseWorker.Controllers;
using DatabaseWorker.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DbWorker.Test
{
	[TestClass]
	public class DbWorkerTests
	{
		[TestMethod]
		public void AddTwoDifferentEntities_ShouldNotFail()
		{
			var usr1 = new User()
			{
				VkId = 123123,
				Group = new HashSet<Group>(),
				Post = new HashSet<Post>(),
				Friends = new HashSet<User>(),
				AvatarLink100 = "---",
				AvatarLink50 = "---",
				FirstName = "aaa",
				IsHidden = true,
				LastName = "lll"
			};
			var usr2 = new User()
			{
				VkId = 121111,
				Group = new HashSet<Group>(),
				Post = new HashSet<Post>(),
				Friends = new HashSet<User>(),
				AvatarLink100 = "---",
				AvatarLink50 = "---",
				FirstName = "aaa",
				IsHidden = true,
				LastName = "lll"
			};

			var controller = new UserController();
			controller.AddOrUpdateEntity(usr1);
			controller.AddOrUpdateEntity(usr2);
		}

		[TestMethod]
		public void AddDuplicateKeyEntity_ShouldNotFail()
		{
			var usr1 = new User()
			{
				VkId = 123123,
				Group = new HashSet<Group>(),
				Post = new HashSet<Post>(),
				Friends = new HashSet<User>(),
				AvatarLink100 = "---",
				AvatarLink50 = "---",
				FirstName = "aaa",
				IsHidden = true,
				LastName = "lll"
			};
			var usr2 = new User()
			{
				VkId = 123123,
				Group = new HashSet<Group>(),
				Post = new HashSet<Post>(),
				Friends = new HashSet<User>(),
				AvatarLink100 = "-2351-",
				AvatarLink50 = "-324",
				FirstName = "a235a",
				IsHidden = true,
				LastName = "352ll"
			};

			var controller = new UserController();
			controller.AddOrUpdateEntity(usr1);
			controller.AddOrUpdateEntity(usr2);
		}

		[TestMethod]
		public void AddUserWithTheSameUserFriend_ShouldNotFail()
		{
			var usr1 = new User()
			{
				VkId = 111,
				Group = new HashSet<Group>(),
				Post = new HashSet<Post>(),
				Friends = new HashSet<User>(),
				AvatarLink100 = "---",
				AvatarLink50 = "---",
				FirstName = "aaa",
				IsHidden = true,
				LastName = "lll"
			};
			var usr2 = new User()
			{
				VkId = 222,
				Group = new HashSet<Group>(),
				Post = new HashSet<Post>(),
				Friends = new HashSet<User>() { usr1 },
				AvatarLink100 = "-2351-",
				AvatarLink50 = "-324",
				FirstName = "a235a",
				IsHidden = true,
				LastName = "352ll"
			};

			var controller = new UserController();
			controller.AddOrUpdateEntity(usr1);
			controller.AddOrUpdateEntity(usr2);
		}

		[TestMethod]
		public void AddUsersWithAFriend_ShouldNotFail()
		{
			var friend = new User()
			{
				VkId = 11,
				Group = new HashSet<Group>(),
				Post = new HashSet<Post>(),
				Friends = new HashSet<User>(),
				AvatarLink100 = "---",
				AvatarLink50 = "---",
				FirstName = "aaa",
				IsHidden = true,
				LastName = "lll"
			};

			var usr1 = new User()
			{
				VkId = 22,
				Group = new HashSet<Group>(),
				Post = new HashSet<Post>(),
				Friends = new HashSet<User> { friend },
				AvatarLink100 = "---",
				AvatarLink50 = "---",
				FirstName = "aaa",
				IsHidden = true,
				LastName = "lll"
			};
			var usr2 = new User()
			{
				VkId = 33,
				Group = new HashSet<Group>(),
				Post = new HashSet<Post>(),
				Friends = new HashSet<User> { friend },
				AvatarLink100 = "-2351-",
				AvatarLink50 = "-324",
				FirstName = "a235a",
				IsHidden = true,
				LastName = "352ll"
			};

			var controller = new UserController();
			controller.AddOrUpdateEntity(usr1);
			controller.AddOrUpdateEntity(usr2);
		}

		[TestMethod]
		public void AddUsersWithFriends_ShouldNotFail()
		{
			var controller = new UserController();
			var friend = new User()
			{
				VkId = 1,
				Group = new HashSet<Group>(),
				Post = new HashSet<Post>(),
				Friends = new HashSet<User>(),
				AvatarLink100 = "---",
				AvatarLink50 = "---",
				FirstName = "aaa",
				IsHidden = true,
				LastName = "lll"
			};

			var usr1 = new User()
			{
				VkId = 2,
				Group = new HashSet<Group>(),
				Post = new HashSet<Post>(),
				Friends = new HashSet<User> { friend },
				AvatarLink100 = "---",
				AvatarLink50 = "---",
				FirstName = "aaa",
				IsHidden = true,
				LastName = "lll"
			};

			controller.AddOrUpdateEntity(usr1);

			var usr2 = new User()
			{
				VkId = 3,
				Group = new HashSet<Group>(),
				Post = new HashSet<Post>(),
				Friends = new HashSet<User> { friend, usr1 },
				AvatarLink100 = "-2351-",
				AvatarLink50 = "-324",
				FirstName = "a235a",
				IsHidden = true,
				LastName = "352ll"
			};
			
			controller.AddOrUpdateEntity(usr2);
		}

		[TestMethod]
		public void AddRelationsBetweenUsers_ShouldNotFail()
		{
			using (var controller = new UserController())
			{

				var usr1 = new User()
				{
					VkId = 1,
					Group = new HashSet<Group>(),
					Post = new HashSet<Post>(),
					Friends = new HashSet<User> {},
					AvatarLink100 = "---",
					AvatarLink50 = "---",
					FirstName = "aaa",
					IsHidden = true,
					LastName = "lll"
				};

				controller.AddOrUpdateEntity(usr1);

				var usr2 = new User()
				{
					VkId = 2,
					Group = new HashSet<Group>(),
					Post = new HashSet<Post>(),
					Friends = new HashSet<User> {},
					AvatarLink100 = "-2351-",
					AvatarLink50 = "-324",
					FirstName = "a235a",
					IsHidden = true,
					LastName = "352ll"
				};

				controller.AddOrUpdateEntity(usr2);

				var db1 = controller.GetEntityById(1);
				var db2 = controller.GetEntityById(1);

				
			}

		}

	}
}
