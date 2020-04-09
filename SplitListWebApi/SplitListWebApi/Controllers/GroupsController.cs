using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Areas.Identity.Data.Models;
using SplitListWebApi.Models;
using SplitListWebApi.Repository;

namespace SplitListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private SplitListContext _context;
        private IGroupRepository repo;

        public GroupsController(SplitListContext context)
        {
            _context = context;
            repo = new GroupRepository(_context);
        }

        //Get: api/groups/5
        //Returns GroupDTO from specific ID.
        [HttpGet("{id}")]
        public GroupDTO GetGroupByID(int id)
        {
            return repo.GetGroupByGroupID(id);
        }

        //Get: api/groups/5/owner
        //Returns the UserDTO of the owner of the group specified by an ID.
        [HttpGet("{id}/owner")]
        public UserDTO GetOwnerOfGroup(int id)
        {
            GroupDTO group = repo.GetGroupByGroupID(id);
            return repo.GetOwnerOfGroup(group);
        }

        //Post: api/groups
        //Updates/Creates a group and its members.
        [HttpPost]
        public GroupDTO UpdateGroup(GroupDTO group)
        {
            return repo.UpdateGroup(group);
        }

        //Delete: api/groups
        //Deletes group from db.
        [HttpDelete]
        public void DeleteGroup(GroupDTO group)
        {
            repo.DeleteGroup(group);
        }
    }
}