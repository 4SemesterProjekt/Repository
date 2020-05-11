using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.User;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Services.Interfaces;

namespace SplitListWebApi.Services
{
    public class UserService : IPublicService<UserDTO, UserModel>, IModelService<UserDTO, UserModel>
    {
        private SplitListContext _context;
        private IMapper _mapper;
        private GenericRepository<UserModel> _userRepo;
        private UserGroupRepository _ugRepo;
        public UserService(SplitListContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _userRepo = new GenericRepository<UserModel>(context);
            _ugRepo = new UserGroupRepository(context);
        }

        public List<UserDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserDTO GetBy(Expression<Func<UserModel, bool>> predicate)
        {
            return _mapper.Map<UserDTO>(GetModels(predicate).FirstOrDefault());
        }

        public IEnumerable<UserModel> GetModels(Expression<Func<UserModel, bool>> predicate, bool disableTracking = true)
        {
            return _userRepo.GetBy(
                selector: source => source,
                predicate: predicate,
                include: source => source.Include(um => um.UserGroups)
                    .ThenInclude(ug => ug.GroupModel),
                disableTracking: disableTracking);
        }

        public UserDTO Create(UserDTO dto)
        {
            throw new Exception
                (
                "Tried to create user through UserController but" +
                " should have been AccountController. Check if URL is correct."
                );
        }

        public UserDTO Update(UserDTO dto)
        {
            var model = _mapper.Map<UserModel>(dto);
            var dbModel = GetModels(source => source.Id == model.Id, false).FirstOrDefault();
            if(dbModel == null) throw new NullReferenceException("User not found in database.");

            _ugRepo.DeleteUserGroups(dbModel.UserGroups);
            _ugRepo.CreateUserGroups(_mapper.Map<List<GroupModel>>(dto.Groups), dbModel);

            dbModel.Name = dto.Name;
            _userRepo.Update(dbModel);

            dbModel = GetModels(source => source.Id == model.Id).FirstOrDefault();

            return _mapper.Map<UserDTO>(dbModel);
        }

        public void Delete(UserDTO dto)
        {
            var model = _mapper.Map<UserModel>(dto);
            var dbModel = GetModels(source => source.Id == model.Id, false).FirstOrDefault();
            if (dbModel == null) throw new NullReferenceException("UserDTO wasn't found in the database when trying to delete.");

            _ugRepo.DeleteUserGroups(dbModel.UserGroups);
            _userRepo.Delete(dbModel);
        }
    }
}