using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.ShadowTables;
using ApiFormat.User;
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
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
            return _mapper.Map<GroupDTO>(groupRepo.GetBy(
                selector: source => source,
                predicate: model => model.ModelId == id,
                include: source =>
                    source
                        .Include(gm => gm.UserGroups)
                            .ThenInclude(ug => ug.UserModel)
                        .Include(gm => gm.ShoppingLists)
                        .Include(gm => gm.PantryModel)));
        }

        public GroupDTO Create(GroupDTO dto)
        {
            var model = _mapper.Map<GroupModel>(dto);
            var dbModel = groupRepo.GetBy(
                selector: source => source,
                predicate: gm => gm.ModelId == model.ModelId,
                include: source =>
                    source.Include(groupModel => groupModel.UserGroups)
                        .ThenInclude(ug => ug.UserModel));

            if (dbModel == null)
            {
                dbModel = groupRepo.Create(model);

                _ugRepo.CreateUserGroups(dbModel, dto.Users);
                dbModel.UserGroups = _ugRepo.GetBy(dbModel.ModelId, dto.Users);

                return _mapper.Map<GroupDTO>(dbModel);
            }

            return _mapper.Map<GroupDTO>(dbModel);
        }

        public GroupDTO Update(GroupDTO dto)
        {
            var model = _mapper.Map<GroupModel>(dto);
            var dbModel = groupRepo.GetBy(
                selector: m => m,
                predicate: gm => gm.ModelId == model.ModelId,
                include: source =>
                    source.Include(groupModel => groupModel.UserGroups)
                        .ThenInclude(ug => ug.UserModel),
                disableTracking: false);

            if (dbModel == null)
                return _mapper.Map<GroupDTO>(groupRepo.Create(model));

            //TODO: Update Property Function
            //Update Properties (missing pantry & SL)
            dbModel.Name = dto.Name;
            dbModel.OwnerId = dto.OwnerID;

            _ugRepo.DeleteUserGroups(dbModel.UserGroups);
            _ugRepo.CreateUserGroups(dbModel, dto.Users);
            return _mapper.Map<GroupDTO>(dbModel);
        }

        public void Delete(GroupDTO dto)
        {
            var model = _mapper.Map<GroupModel>(dto);
            var dbModel = groupRepo.GetBy(
                selector: source => source,
                predicate: gm => gm.ModelId == model.ModelId,
                include: source =>
                    source.Include(groupModel => groupModel.UserGroups)
                        .ThenInclude(ug => ug.UserModel),
                disableTracking: false);
            if (dbModel == null) throw new NullReferenceException("GroupDTO wasn't found in the database when trying to delete.");

            _ugRepo.DeleteUserGroups(dbModel.UserGroups);
            groupRepo.Delete(dbModel);
        }
    }
}