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
            return _mapper.Map<GroupDTO>(groupRepo.GetBy(model => model.ModelId == id));
        }

        public GroupDTO Create(GroupDTO dto)
        {
            var model = _mapper.Map<GroupModel>(dto);
            var dbModel = groupRepo.GetBy(m => m.ModelId == model.ModelId);
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
            var dbModel = groupRepo.GetBy(m => m.ModelId == model.ModelId); //Denne ignorer UserGroups. Derfor giver "DeleteUSerGroups" nedenunder ingen mening

            if (dbModel == null)
                return _mapper.Map<GroupDTO>(groupRepo.Create(model));

            //Update Properties (missing pantry & SL)
            dbModel.Name = dto.Name;
            dbModel.OwnerId = dto.OwnerID;

            //mangler query til at hente UserGroups
            dbModel.UserGroups = _context.UserGroups
                .Include(ug => ug.UserModel)
                .Include(ug => ug.GroupModel)
                .Where(ug => ug.GroupModelModelID == dbModel.ModelId)
                .ToList(); //Query må gerne omformuleres eller skrives til at anvende repo

            _ugRepo.DeleteUserGroups(dbModel.UserGroups); //Virker aldrig pga ovenstående mapping
            _ugRepo.CreateUserGroups(dbModel, dto.Users);
            return _mapper.Map<GroupDTO>(dbModel);
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