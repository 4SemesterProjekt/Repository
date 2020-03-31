using ApiFormat;
using SplitListWebApi.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SplitListWebApi.Repository
{
    public interface IGroupRepository
    {
        List<UserDTO> GetUsersInGroup(GroupDTO group);
        UserDTO GetOwnerOfGroup(GroupDTO group);
        void AddGroup(GroupDTO group);
        void UpdateGroup(GroupDTO group);
        void DeleteGroup(GroupDTO group);
        GroupDTO GetGroupByGroupID(int ID);
    }

    public class GroupRepository : IGroupRepository
    {
        private SplitListContext context;
        public GroupRepository(SplitListContext Context)
        {
            context = Context;
        }

        public UserDTO GetOwnerOfGroup(GroupDTO group)
        {
            Group dbGroup = LoadToModel(group);
            User owner = context.Users.Where(u => u.UserID == group.OwnerID).FirstOrDefault();
            if (owner != null)
            {
                return new UserDTO()
                {
                    Name = owner.Name,
                    UserID = owner.UserID
                };
            }
            else return null;
        }
        public void UpdateGroup(GroupDTO group)
        {
            Group dbGroup = LoadToModel(group);
            if (dbGroup != null)
            {
                dbGroup.Name = group.Name;
                dbGroup.OwnerID = group.OwnerID;

                if (group.Users == null)
                {
                    group.Users = new List<UserDTO>();
                }
                else
                {
                    List<UserGroup> dbUserInGroup = context.UserGroups
                        .Where(ug => ug.GroupID == group.GroupID)
                        .Include(ug => ug.User)
                        .ToList();

                    foreach (UserGroup dbUserGroup in dbUserInGroup)
                    {
                        group.Users.Add(new UserDTO()
                        {
                            Name = dbUserGroup.User.Name,
                            UserID = dbUserGroup.User.UserID
                        });
                        context.SaveChanges();
                    }
                }
            }
        }
        public void AddGroup(GroupDTO group)
        {
            LoadToModel(group);
        }
        public List<UserDTO> GetUsersInGroup(GroupDTO group)
        {
            List<UserDTO> users = new List<UserDTO>();

            List<UserGroup> userGroups = context.Groups.Where(g => g.GroupID == group.GroupID)
                .Include(u => u.UserGroups)
                .ThenInclude(ug => ug.User)
                .SelectMany(u => u.UserGroups)
                .ToList();

            if (userGroups != null)
            {
                foreach (UserGroup usergroup in userGroups)
                {
                    users.Add(new UserDTO()
                    {
                        Name = usergroup.User.Name,
                        UserID = usergroup.User.UserID
                    });
                }
            }
            return users;
        }

        private Group LoadToModel(GroupDTO group) 
        {
            Group dbGroup = context.Groups.Find(group.GroupID);

            if (dbGroup != null)
                return dbGroup;
            else
            {
                Group newGroup = new Group()
                {
                    Name = group.Name,
                    OwnerID = group.OwnerID
                };
                context.Groups.Add(newGroup);
                context.SaveChanges();
                return newGroup;
            }
        }

        public void DeleteGroup(GroupDTO group)
        {
            Group dbGroup = context.Groups.Find(group.GroupID);

            if (dbGroup != null)
            {
                context.Groups.Remove(dbGroup);
                context.SaveChanges();

                if (group.Pantry != null)
                {
                    PantryRepository tempRepo = new PantryRepository(context);
                    tempRepo.DeletePantry(group.Pantry);
                }
            }
        }

        public GroupDTO GetGroupByGroupID(int ID)
        {
            Group dbGroup = context.Groups.Find(ID);
            if (dbGroup != null)
            {
                GroupDTO group = new GroupDTO()
                {
                    Name = dbGroup.Name,
                    OwnerID = dbGroup.OwnerID,
                    GroupID = dbGroup.GroupID,
                    ShoppingLists = new List<ShoppingListDTO>()
                };

                group.Users = GetUsersInGroup(group);
                PantryRepository tempPantryRepo = new PantryRepository(context);
                group.Pantry = tempPantryRepo.GetPantryFromGroupID(ID);
                ShoppingListRepository tempShoppinglistRepo = new ShoppingListRepository(context);
                group.ShoppingLists = tempShoppinglistRepo.GetShoppingListsByGroupID(ID);

                return group;
            }
            else return null;
        }
    }
}
