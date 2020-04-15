using System;
using ApiFormat;
using ApiFormat.Group;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Utilities;

namespace SplitListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private SplitListContext _context;
        private GenericRepository<GroupDTO, GroupModel> _repository;

        public GroupsController(SplitListContext context, IMapper mapper)
        {
            _context = context;
            _repository = new GenericRepository<GroupDTO, GroupModel>(_context, mapper);
        }

        [HttpPost("Create")]
        public GroupDTO Create([FromBody] GroupDTO dto)
        {
            return dto.Add(_repository);
        }

        [HttpGet("{id}")]
        public GroupDTO GetById(int id)
        {
            GroupDTO dto = new GroupDTO();
            return dto.GetById(_repository, id);
        }

        [HttpDelete("Delete")]
        public void Delete([FromBody] GroupDTO dto)
        {
            dto.Delete(_repository);
        }

        [HttpPost("Save")]
        public GroupDTO Save(GroupDTO dto)
        {
            return dto.Save(_repository);
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ApiFormat;
//using ApiFormat.User;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SplitListWebApi.Areas.Identity.Data;
//using SplitListWebApi.Repository;

//namespace SplitListWebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class GroupsController : ControllerBase
//    {
//        private SplitListContext _context;
//        private IGroupRepository repo;

//        public GroupsController(SplitListContext context)
//        {
//            _context = context;
//            repo = new GenericRepository(_context);
//        }

//        //Get: api/groups/5
//        //Returns GroupDTO from specific ID.
//        [HttpGet("{id}")]
//        public IGroupDTO GetGroupByID(int id)
//        {
//            return repo.GetGroupByGroupID(id);
//        }

//        //Get: api/groups/5/owner
//        //Returns the IUserDTO of the owner of the group specified by an ID.
//        [HttpGet("{id}/owner")]
//        public IUserDTO GetOwnerOfGroup(int id)
//        {
//            IGroupDTO group = repo.GetGroupByGroupID(id);
//            return repo.GetOwnerOfGroup(group);
//        }

//        //Post: api/groups
//        //Updates/Creates a group and its members.
//        [HttpPost]
//        public IGroupDTO UpdateGroup(IGroupDTO group)
//        {
//            return repo.UpdateGroup(group);
//        }

//        //Delete: api/groups
//        //Deletes group from db.
//        [HttpDelete]
//        public void DeleteGroup(IGroupDTO group)
//        {
//            repo.DeleteGroup(group);
//        }
//    }
//}