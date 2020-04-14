using System;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Interfaces;
using SplitListWebApi.Utilities;

namespace SplitListWebApi.Repositories.Implementation
{
    public class GenericRepository<TSource, TEntity> : IRepository<TSource>
        where TSource : class, IDTO
        where TEntity : class, IModel
    {
        private readonly SplitListContext _dbContext;
        private IMapper _mapper;
        public GenericRepository(SplitListContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /*
         * General for functions:
         * Map to Model-Entity
         * Run DB-function
         * Model back to DTO
         * return DTO.
         */

        public IQueryable<TSource> GetAll()
        {
            return _mapper.Map<IQueryable<TSource>>(_dbContext.Set<TEntity>().AsNoTracking());
        }

        public TSource GetById(double id)
        {
            var entity = Query().SingleOrDefault(i => Math.Abs(i.Id - id) < (double)0.1);
            _dbContext.Entry(entity).State = EntityState.Detached; //AsNoTracking().
            var dto = _mapper.Map<TSource>(entity);
            return dto;
        }

        private IQueryable<TEntity> Query(bool eager = true)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            if (eager)
            {
                foreach (var property in _dbContext.Model.FindEntityType(typeof(TEntity)).GetNavigations())
                    query = query.Include(property.Name);
            }
            return query;
        }

        public TSource Create(TSource entity)
        {
            var model = _mapper.Map<TEntity>(entity);
            model.BeginTransaction(_dbContext.Add, _dbContext);
            return _mapper.Map<TSource>(model);
        }

        public EntityEntry<TSource> Update(TSource entity)
        {
            var model = _mapper.Map<TEntity>(entity);
            model.BeginTransaction(_dbContext.Update, _dbContext);
            return _mapper.Map<EntityEntry<TSource>>(_dbContext.Entry(model)); //To check whether any entries has been updated. Look in DTOUtilities.Update.
        }

        public void Delete(TSource entity)
        {
            var model = _mapper.Map<TEntity>(entity);
            model.BeginTransaction(_dbContext.Remove, _dbContext);
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
//                .Where(ug => ug.GroupModelId == group.Id)
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
//                        GroupModelId = group.Id
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
//                    IGroupDTO.ShoppingLists = new List<ShoppingListDTO>()
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
