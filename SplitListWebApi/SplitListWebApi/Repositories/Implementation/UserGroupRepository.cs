using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.ShadowTables;
using ApiFormat.User;
using Microsoft.EntityFrameworkCore;
using SplitListWebApi.Areas.Identity.Data;

namespace SplitListWebApi.Repositories.Implementation
{
    public class UserGroupRepository
    {
        private SplitListContext _context;

        public UserGroupRepository(SplitListContext context) => _context = context;

        public void CreateUserGroups(GroupModel groupModel, List<UserModel> userModels)
        {
            if (userModels != null)
            {
                foreach (var userModel in userModels)
                {
                    if (_context.UserGroups.Find(groupModel.ModelId, userModel.Id) == null)
                    {
                        var entry = _context.UserGroups.Add(new UserGroup()
                        {
                            GroupModel = groupModel,
                            UserModel = userModel
                        });
                        // Hvis vi kun sætter ID kastes exception om foreign key failed.
                        _context.SaveChanges();
                    }
                }
            }
        }

        public void CreateUserGroups(List<GroupModel> groupModels, UserModel userModel)
        {
            foreach (var groupModel in groupModels)
            {
                _context.UserGroups.Add(new UserGroup()
                {
                    GroupModel = groupModel,
                    UserModel = userModel,
                    GroupModelModelID = groupModel.ModelId,
                    UserId = userModel.Id
                });
            }
            _context.SaveChanges();
        }

        public void DeleteUserGroups(List<UserGroup> userGroups)
        {
            if (userGroups != null)
            {
                _context.UserGroups.RemoveRange(userGroups);
                _context.SaveChanges();
            }
        }

        public List<UserGroup> GetBy(int groupId, IEnumerable<string> userIds)
        {
            var userGroups = new List<UserGroup>();
            foreach (var userId in userIds)
            {
                userGroups.Add(_context.UserGroups.FirstOrDefault(ug => ug.GroupModelModelID == groupId && ug.UserId == userId));
            }

            return userGroups;
        }

        public List<UserGroup> GetBy(IEnumerable<int> groupIds, string userId)
        {
            var userGroups = new List<UserGroup>();
            foreach (var groupId in groupIds)
            {
                userGroups.Add(_context.UserGroups.FirstOrDefault(ug => ug.GroupModelModelID == groupId && ug.UserId == userId));
            }

            return userGroups;
        }
    }
}