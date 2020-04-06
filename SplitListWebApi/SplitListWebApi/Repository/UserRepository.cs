using System.Collections.Generic;
using System.Linq;
using SplitListWebApi.Models;
using ApiFormat;
using Microsoft.EntityFrameworkCore;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Areas.Identity.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace SplitListWebApi.Repository
{
    public interface IUserRepository
    {
        List<GroupDTO> GetUsersGroups(string userID);
        void DeleteUser(UserDTO user);
        UserDTO UpdateUser(UserDTO user);
    }

    public class UserRepository : IUserRepository
    {
        private UserManager<User> _userManager;
        private SplitListContext _context;
        public UserRepository(UserManager<User> userManager, SplitListContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<GroupDTO> GetUsersGroups(string userID)
        {
            User dbuser = _userManager.FindByIdAsync(userID).Result;
            if (dbuser != null)
            {
                //May be flawed and only return 1 single group
                List<Group> userGroups = _context.UserGroups
                    .Where(ug => ug.Id == userID)
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
            User dbUser =_userManager.FindByIdAsync(user.Id).Result;
            if (dbUser != null)
                return dbUser;
            else return null;
        }

        public void DeleteUser(UserDTO user)
        {
            User dbUser = LoadToModel(user);
            if (dbUser != null)
            {
                _userManager.DeleteAsync(dbUser);

                foreach (GroupDTO group in GetUsersGroups(user.Id))
                {
                    UserGroup toDelete = _context.UserGroups.Find(user.Id, group.GroupID);
                    if (toDelete != null)
                        _context.UserGroups.Remove(toDelete);
                }
                _context.SaveChanges();
            }
                

        }

        public UserDTO UpdateUser(UserDTO user)
        {
            User dbUser = LoadToModel(user);
            if (dbUser != null)
            {
                var result = _userManager.UpdateAsync(dbUser).Result;
                return new UserDTO()
                {
                    Groups = user.Groups,
                    Id = dbUser.Id,
                    Name = dbUser.Name
                };
            }
            else return null;
            
        }
    }
}