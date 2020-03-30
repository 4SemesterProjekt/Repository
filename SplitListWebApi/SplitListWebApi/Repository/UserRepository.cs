using System.Collections.Generic;
using System.Linq;
using SplitListWebApi.Models;
using ApiFormat;
using Microsoft.EntityFrameworkCore;

namespace SplitListWebApi.Repository
{
    public interface IUserRepository
    {
        List<GroupDTO> GetUsersGroups(UserDTO user);
        void AddUser(UserDTO user);
        void DeleteUser(UserDTO user);
        void UpdateUser(UserDTO user);
    }

    public class UserRepository : IUserRepository
    {
        public SplitListContext context { get; set; }
        public UserRepository(SplitListContext context_)
        {
            context = context_;
        }

        public List<GroupDTO> GetUsersGroups(UserDTO user)
        {
            User dbuser = LoadToModel(user);
            if (dbuser != null)
            {
                //May be flawed and only return 1 single group
                List<Group> userGroups = context.UserGroups
                    .Where(ug => ug.UserID == user.UserID)
                    .Include(g => g.Group)
                    .Select(ug => ug.Group)
                    .ToList();

                List<GroupDTO> userGroupDTO = new List<GroupDTO>();

                foreach (Group group in userGroups)
                {
                    userGroupDTO.Add(new GroupDTO()
                    {
                        GroupID = group.GroupID,
                        OwnerID = group.OwnerID,
                        Name = group.Name
                    });
                }
                return userGroupDTO;
            }
            else return null;
        }

        private User LoadToModel(UserDTO user)
        {
            User dbUser = context.Users.Find(user.UserID);

            if (dbUser != null)
                return dbUser;
            else
            {
                User newUser = new User()
                {
                    Name = user.Name
                };
                context.Users.Add(newUser);
                context.SaveChanges();
                return newUser;
            }
        }

        public void AddUser(UserDTO newuser)
        {
            LoadToModel(newuser);
        }

        public void DeleteUser(UserDTO user)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateUser(UserDTO user)
        {
            throw new System.NotImplementedException();
        }
    }
}