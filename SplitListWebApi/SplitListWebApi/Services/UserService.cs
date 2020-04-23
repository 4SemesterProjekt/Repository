using System;
using System.Collections.Generic;
using System.Linq;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.User;
using AutoMapper;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Services.Interfaces;

namespace SplitListWebApi.Services
{
    public class UserService : IService<UserDTO, string>
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

        public UserDTO GetById(string id)
        {
            return _mapper.Map<UserDTO>(_userRepo.GetBy(model => model.Id == id));
        }

        public UserDTO Create(UserDTO dto)
        {
            throw new NotImplementedException();
        }

        public UserDTO Update(UserDTO dto)
        {
            var model = _mapper.Map<UserModel>(dto);
            var dbModel = _userRepo.GetBy(model => model.Id == dto.Id);
            if(dbModel == null) throw new NullReferenceException("User not found in database.");

            _ugRepo.DeleteUserGroups(dbModel.UserGroups);
            _ugRepo.CreateUserGroups(_mapper.Map<List<GroupModel>>(dto.Groups), dbModel);

            dbModel = _userRepo.GetBy(model => model.Id == dto.Id);
            dbModel.UserGroups = _ugRepo.GetBy(dbModel.UserGroups.Select(ug => ug.GroupModelModelID), dbModel.Id);
            return _mapper.Map<UserDTO>(dbModel);
        }

        public void Delete(UserDTO entity)
        {
            throw new System.NotImplementedException();
        }
    }
}