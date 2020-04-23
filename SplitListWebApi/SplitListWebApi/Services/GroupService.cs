using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.ShadowTables;
using ApiFormat.User;
using AutoMapper;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Services.Interfaces;
using SQLitePCL;

namespace SplitListWebApi.Services
{
    public class GroupService : IService<GroupDTO, int>
    {
        private SplitListContext _context;
        private IMapper _mapper;
        private GenericRepository<GroupModel> groupRepo;
        private UserGroupRepository _ugRepo;

        public GroupService(SplitListContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            groupRepo = new GenericRepository<GroupModel>(_context);
            _ugRepo = new UserGroupRepository(_context);
        }

        public GroupDTO GetById(int id)
        {
            return _mapper.Map<GroupDTO>(groupRepo.GetBy(model => model.ModelId == id));
        }

        public GroupDTO Create(GroupDTO dto)
        {
            var model = _mapper.Map<GroupModel>(dto);
            var dbModel = groupRepo.GetBy(model => model.ModelId == dto.ModelId);

            if (dbModel == null)
            {
                var createdGroupModel = groupRepo.Create(model);
                var userModels = _mapper.Map<List<UserModel>>(dto.Users);
                _ugRepo.CreateUserGroups(createdGroupModel, userModels);

                dbModel = groupRepo.GetBy(model => model.ModelId == createdGroupModel.ModelId);
                dbModel.UserGroups = _ugRepo.GetBy(dbModel.ModelId, userModels.Select(u => u.Id));
                return _mapper.Map<GroupDTO>(dbModel);
            }

            return _mapper.Map<GroupDTO>(dbModel);
        }

        public GroupDTO Update(GroupDTO dto)
        {
            var model = _mapper.Map<GroupModel>(dto);
            var dbModel = groupRepo.GetBy(model => model.ModelId == dto.ModelId);
            if (dbModel == null)
                return _mapper.Map<GroupDTO>(groupRepo.Create(model));

            _ugRepo.DeleteUserGroups(dbModel.UserGroups);
            _ugRepo.CreateUserGroups(dbModel, _mapper.Map<List<UserModel>>(dto.Users));
            return _mapper.Map<GroupDTO>(groupRepo.GetBy(model => model.ModelId == dbModel.ModelId));
        }

        public void Delete(GroupDTO dto)
        {
            var model = _mapper.Map<GroupModel>(dto);
            var dbModel = groupRepo.GetBy(model => model.ModelId == dto.ModelId);
            if (dbModel == null) return;

            _ugRepo.DeleteUserGroups(dbModel.UserGroups);
            groupRepo.Delete(dbModel);
        }
    }
}