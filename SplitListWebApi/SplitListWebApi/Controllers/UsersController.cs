using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Models;
using SplitListWebApi.Repository;

namespace SplitListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SplitListContext _context;
        private UserRepository repo;

        public UsersController(SplitListContext context)
        {
            _context = context;
        }

        //GET: User's Groups
        [HttpGet("/{id}/groups")]
        public List<IGroupDTO> GetGroupsFromUser(string id)
        {
            return repo.GetUsersGroups(id);
        }

        // POST: api/Users
        [HttpPost]
        public UserDTO UpdateUser(UserDTO user)
        {
            return repo.UpdateUser(user);
        }

        // DELETE: api/Users
        [HttpDelete]
        public void DeleteUser(UserDTO user)
        {
            repo.DeleteUser(user);
        }

        

    }
}
