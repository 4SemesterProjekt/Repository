using System;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Controllers.Utilities;
using SplitListWebApi.Repositories.Interfaces;

namespace SplitListWebApi.Repositories.Implementation
{
    public class GroupRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private readonly SplitListContext _dbContext;
        public GroupRepository(SplitListContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }

        public TEntity GetById(double id)
        {
            var entity = _dbContext.Find<TEntity>(id);
            _dbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public TEntity Create(TEntity entity)
        {
            entity.BeginTransaction(_dbContext.Add<TEntity>, _dbContext);
            return entity;
        }

        public EntityEntry<TEntity> Update(TEntity entity)
        {
            entity.BeginTransaction(_dbContext.Update<TEntity>, _dbContext);
            return _dbContext.Entry(entity); //To check whether any entries has been updated. Look in DTOUtilities.Update.
        }

        public void Delete(double id)
        {
            var entity = GetById(id);
            entity.BeginTransaction(_dbContext.Remove<TEntity>, _dbContext);
        }
    }
}

//using ApiFormat;
//using System.Collections.Generic;
//using Microsoft.EntityFrameworkCore;
//using System.Linq;
//using ApiFormat.ShadowTables;
//using ApiFormat.ShoppingList;
//using ApiFormat.User;
//using SplitListWebApi.Areas.Identity.Data;

//namespace SplitListWebApi.Repository
//{
//    public interface IGroupRepository
//    {
//        //List<IUserDTO> GetUsersInGroup(GroupDTO group);
//        IUserDTO GetOwnerOfGroup(IGroupDTO group);
//        IGroupDTO UpdateGroup(IGroupDTO group);
//        void DeleteGroup(IGroupDTO group);
//        IGroupDTO GetGroupByGroupID(int ID);
//    }

//    public class GroupRepository : IGroupRepository
//    {
//        private SplitListContext context;
//        public GroupRepository(SplitListContext Context)
//        {
//            context = Context;
//        }

//        public IUserDTO GetOwnerOfGroup(IGroupDTO group)
//        {
//            Group dbGroup = LoadToModel(group);
//            User owner = context.Users.FirstOrDefault(u => u.Id == @group.Id);
//            if (owner != null)
//            {
//                return new User()
//                {
//                    Name = owner.Name,
//                    Id = owner.Id
//                };
//            }
//            else return null;
//        }

//        private void RemoveUsersFromGroup(IGroupDTO group)
//        {
//            List<UserGroup> dbUsersInGroup = context.UserGroups
//                .Where(ug => ug.GroupID == group.Id)
//                .Include(ug => ug.User)
//                .ToList();

//            foreach (UserGroup userGroup in dbUsersInGroup)
//            {
//                IUserDTO userToRemove = group.Users.Find(u => u.Id == userGroup.UserID);
//                if (userToRemove == null)
//                {
//                    context.UserGroups.Remove(userGroup);
//                }
//            }
//        }

//        private void AddUsersToGroup(IGroupDTO group)
//        {
//            foreach (IUserDTO user in group.Users)
//            {
//                User userModel = context.Users.Find(user.Id);
//                if (userModel != null)
//                {
//                    userModel.Name = user.Name;
//                    context.Users.Update(userModel);
//                    context.SaveChanges();
//                }
//                else
//                {
//                    // Modify to complete microsoft identity ( use user repo )
//                    context.Users.Add(new User()
//                    {
//                        Id = user.Id,
//                        Name = user.Name
//                    });
//                    context.SaveChanges();
//                }

//                UserGroup userGroupModel = context.UserGroups.Find(user.Id, group.Id);
//                if (userGroupModel == null)
//                {
//                    context.UserGroups.Add(new UserGroup()
//                    {
//                        UserID = user.Id,
//                        GroupID = group.Id
//                    });
//                    context.SaveChanges();
//                }
//            }
//        }

//        public IGroupDTO UpdateGroup(IGroupDTO group)
//        {
//            Group dbGroup = LoadToModel(group);
//            if (group.Id <= 0)
//            {
//                group.Id = dbGroup.Id;
//            }

//            dbGroup.Name = group.Name;
//            dbGroup.OwnerID = group.OwnerID;

//            if (group.Users == null)
//            {
//                group.Users = new List<IUserDTO>();
//            }
//            else
//            {
//                RemoveUsersFromGroup(group);
//                AddUsersToGroup(group);
//            }
//            return group;
//        }

//        public List<IUserDTO> GetUsersInGroup(IGroupDTO group)
//        {
//            List<IUserDTO> users = new List<IUserDTO>();

//            List<UserGroup> userGroups = context.Groups.Where(g => g.Id == group.Id)
//                .Include(u => u.UserGroups)
//                .ThenInclude(ug => ug.User)
//                .SelectMany(u => u.UserGroups)
//                .ToList();

//            if (userGroups != null)
//            {
//                foreach (UserGroup usergroup in userGroups)
//                {
//                    users.Add(new User()
//                    {
//                        Name = usergroup.User.Name,
//                        Id = usergroup.User.Id
//                    });
//                }
//            }
//            return users;
//        }

//        private Group LoadToModel(IGroupModel group) 
//        {
//            Group dbGroup = context.Groups.Find(group.Id);

//            if (dbGroup != null)
//                return dbGroup;
//            else
//            {
//                Group newGroup = new Group()
//                {
//                    Name = group.Name,
//                    OwnerID = group.OwnerID
//                };
//                context.Groups.Add(newGroup);
//                context.SaveChanges();
//                return newGroup;
//            }
//        }

//        public void DeleteGroup(IGroupDTO group)
//        {
//            Group dbGroup = context.Groups.Find(group.Id);

//            if (dbGroup != null)
//            {
//                context.Groups.Remove(dbGroup);
//                context.SaveChanges();

//                if (group.Pantry != null)
//                {
//                    PantryRepository tempRepo = new PantryRepository(context);
//                    tempRepo.DeletePantry(group.Pantry);
//                }
//            }
//        }

//        public IGroupDTO GetGroupByGroupID(int ID)
//        {
//            Group dbGroup = context.Groups.Find(ID);
//            if (dbGroup != null)
//            {
//                IGroupDTO group = new Group()
//                {
//                    Name = dbGroup.Name,
//                    OwnerID = dbGroup.OwnerID,
//                    Id = dbGroup.Id,
//                    IGroupDTO.ShoppingLists = new List<IShoppingListDTO>()
//                };

//                group.Users = GetUsersInGroup(group);
//                PantryRepository tempPantryRepo = new PantryRepository(context);
//                group.Pantry = tempPantryRepo.GetPantryFromGroupID(ID);
//                ShoppingListRepository tempShoppinglistRepo = new ShoppingListRepository(context);
//                group.ShoppingLists = tempShoppinglistRepo.GetShoppingListsByGroupID(ID);

//                return group;
//            }
//            else return null;
//        }
//    }
//}
