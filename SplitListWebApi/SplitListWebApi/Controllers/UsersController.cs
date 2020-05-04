using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.User;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Services;
using SplitListWebApi.Services.Interfaces;
using SplitListWebApi.Utilities;

namespace SplitListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private SplitListContext _context;
        private IService<UserDTO, UserModel> _userService;

        public UsersController(SplitListContext context, IMapper mapper)
        {
            _context = context;
            _userService = new UserService(_context, mapper);
        }

        //GET: User's Groups
        [HttpGet("{id}")]
        public UserDTO GetUserById(string id)
        {
            return _userService.GetBy(source => source.Id == id);
        }

        [HttpGet("Email/{email}")]
        public UserDTO GetUserByEmail(string email)
        {
            return _userService.GetBy(source => source.Email == email);
        }

        //POST: Update User
        [HttpPut]
        public UserDTO Update([FromBody] UserDTO user)
        {
            return _userService.Update(user);
        }

        //DELETE: Delete User
        [HttpDelete]
        public ActionResult Delete([FromBody] UserDTO user)
        {
            try
            {
                _userService.Delete(user);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
