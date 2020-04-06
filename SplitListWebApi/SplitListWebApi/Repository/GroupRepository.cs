using ApiFormat;
using SplitListWebApi.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Areas.Identity.Data.Models;

namespace SplitListWebApi.Repository
{
    public interface IGroupRepository
    {
        List<UserDTO> GetUsersInGroup(GroupDTO group);
        UserDTO GetOwnerOfGroup(GroupDTO group);
        GroupDTO UpdateGroup(GroupDTO group);
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
            User owner = context.Users.Where(u => u.Id == group.OwnerID).FirstOrDefault();
            if (owner != null)
            {
                return new UserDTO()
                {
                    Name = owner.Name,
                    Id = owner.Id
                };
            }
            else return null;
        }

        private void RemoveUsersFromGroup(GroupDTO group)
        {
            List<UserGroup> dbUsersInGroup = context.UserGroups
                .Where(ug => ug.GroupID == group.GroupID)
                .Include(ug => ug.User)
                .ToList();

            foreach (UserGroup userGroup in dbUsersInGroup)
            {
                UserDTO userToRemove = group.Users.Find(u => u.Id == userGroup.Id);
                if (userToRemove == null)
                {
                    context.UserGroups.Remove(userGroup);
                }
            }
        }

        private void AddUsersToGroup(GroupDTO group)
        {
            foreach (UserDTO user in group.Users)
            {
                User userModel = context.Users.Find(user.Id);
                if (userModel != null)
                {
                    userModel.Name = user.Name;
                    context.Users.Update(userModel);
                    context.SaveChanges();
                }
                else
                {
                    // Modify to complete microsoft identity ( use user repo )
                    context.Users.Add(new User()
                    {
                        Id = user.Id,
                        Name = user.Name
                    });
                    context.SaveChanges();
                }

                UserGroup userGroupModel = context.UserGroups.Find(user.Id, group.GroupID);
                if (userGroupModel == null)
                {
                    context.UserGroups.Add(new UserGroup()
                    {
                        Id = user.Id,
                        GroupID = group.GroupID
                    });
                    context.SaveChanges();
                }
            }
        }

        public GroupDTO UpdateGroup(GroupDTO group)
        {
            Group dbGroup = LoadToModel(group);
            if (group.GroupID <= 0)
            {
                group.GroupID = dbGroup.GroupID;
            }

            dbGroup.Name = group.Name;
            dbGroup.OwnerID = group.OwnerID;

            if (group.Users == null)
            {
                group.Users = new List<UserDTO>();
            }
            else
            {
                RemoveUsersFromGroup(group);
                AddUsersToGroup(group);
            }
            return group;
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
                        Id = usergroup.User.Id
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
