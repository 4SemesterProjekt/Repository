//using System.Collections.Generic;
//using System.Linq;
//using ApiFormat;
//using ApiFormat.ShadowTables;
//using ApiFormat.User;
//using Microsoft.EntityFrameworkCore;
//using SplitListWebApi.Areas.Identity.Data;
//using Microsoft.AspNetCore.Identity;

//namespace SplitListWebApi.Repository
//{
//    public interface IUserRepository
//    {
//        List<IGroupDTO> GetUsersGroups(string userID);
//        void DeleteUser(IUserDTO user);
//        IUserDTO UpdateUser(IUserDTO user);
//    }

//    public class UserRepository : IUserRepository
//    {
//        private UserManager<User> _userManager;
//        private SplitListContext _context;
//        public UserRepository(UserManager<User> userManager, SplitListContext context)
//        {
//            _context = context;
//            _userManager = userManager;
//        }

//        public List<IGroupDTO> GetUsersGroups(int userID)
//        {
//            User dbuser = _userManager.FindByIdAsync(userID.ToString()).Result;
//            if (dbuser != null)
//            {
//                //May be flawed and only return 1 single group
//                List<IGroupModel> userGroups = _context.UserGroups
//                    .Where(ug => ug.UserID == userID)
//                    .Include(g => g.Group)
//                    .Select(ug => ug.Group)
//                    .ToList();

//                List<IGroupDTO> userGroupDTO = new List<IGroupDTO>();

//                foreach (Group group in userGroups)
//                {
//                    userGroupDTO.Add(new Group()
//                    {
//                        Id = group.Id,
//                        OwnerID = group.OwnerID,
//                        Name = group.Name
//                    });
//                }
//                return userGroupDTO;
//            }
//            else return null;
//        }

//        private User LoadToModel(IUserDTO user)
//        {
//            User dbUser =_userManager.FindByIdAsync(user.Id.ToString()).Result;
//            if (dbUser != null)
//                return dbUser;
//            else return null;
//        }

//        public void DeleteUser(IUserDTO user)
//        {
//            User dbUser = LoadToModel(user);
//            if (dbUser != null)
//            {
//                _userManager.DeleteAsync(dbUser);

//                foreach (IGroupDTO group in GetUsersGroups(user.Id))
//                {
//                    UserGroup toDelete = _context.UserGroups.Find(user.Id, group.Id);
//                    if (toDelete != null)
//                        _context.UserGroups.Remove(toDelete);
//                }
//                _context.SaveChanges();
//            }
//        }

//        public IUserDTO UpdateUser(IUserDTO user)
//        {
//            User dbUser = LoadToModel(user);
//            if (dbUser != null)
//            {
//                var result = _userManager.UpdateAsync(dbUser).Result;
//                return new User()
//                {
//                    Groups = user.Groups,
//                    Id = dbUser.Id,
//                    Name = dbUser.Name
//                };
//            }
//            else return null;
            
//        }
//    }
//}