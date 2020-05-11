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
using SplitListWebApi.Utilities;

namespace SplitListWebApi.Repositories.Implementation
{
    public class UserGroupRepository
    {
        private SplitListContext _context;

        public UserGroupRepository(SplitListContext context) => _context = context;

        // Denne metode er ny. Den tjekker users i context mod users der er passed med i DTO'en og laver UserGroups derudfra.
        // Det er ikke den mest effektive metode, men den virker. Der skal nok laves en lignende for User siden af den.
        // Jeg har ladt de to andre CreateUserGroups metoder stå, hvis de nu skulle bruges.
        public void CreateUserGroups(GroupModel groupModel, List<UserDTO> userDtos)
        {
            if (userDtos == null) return;
            foreach (var userModel in userDtos.Select(userDto => _context.Users.FirstOrDefault(um => um.Id == userDto.Id)))
            {
                _context.UserGroups.Add(new UserGroup()
                {
                    GroupModelModelID = groupModel.ModelId,
                    UserId = userModel.Id,
                    GroupModel = groupModel,
                    UserModel = userModel
                });
            }
            _context.SaveChanges();
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
    }
}