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

namespace SplitListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private SplitListContext _context;
        private GenericRepository<UserDTO, UserModel> _repository;

        public UsersController(SplitListContext context, IMapper mapper)
        {
            _context = context;
            _repository = new GenericRepository<UserDTO, UserModel>(_context, mapper);
        }

        //GET: User's Groups
        [HttpGet("/{id}")]
        public UserDTO GetUserById(double id)
        {
            UserDTO dto = new UserDTO();
            return null;
        }
    }
}
