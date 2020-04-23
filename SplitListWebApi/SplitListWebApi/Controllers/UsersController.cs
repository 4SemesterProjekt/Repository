//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ApiFormat;
//using ApiFormat.Group;
//using ApiFormat.User;
//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SplitListWebApi.Areas.Identity.Data;
//using SplitListWebApi.Repositories.Implementation;
//using SplitListWebApi.Utilities;

//namespace SplitListWebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UsersController : ControllerBase
//    {
//        private SplitListContext _context;
//        private GenericRepository<UserDTO, UserModel> _repository;

//        public UsersController(SplitListContext context, IMapper mapper)
//        {
//            _context = context;
//            _repository = new GenericRepository<UserDTO, UserModel>(_context, mapper);
//        }

//        //GET: User's Groups
//        [HttpGet("{id}")]
//        public UserDTO GetUserById(int id)
//        {
//            UserDTO dto = new UserDTO();
//            return dto.GetById(_repository, id);
//        }

//        //POST: Update User
//        [HttpPost("Save")]
//        public UserDTO Save([FromBody] UserDTO user)
//        {
//            return user.Save(_repository);
//        }

//        //DELETE: Delete User
//        [HttpDelete("Delete")]
//        public ActionResult Delete([FromBody] UserDTO user)
//        {
//            user.Delete(_repository);
//            return Ok(user);
//        }
//    }
//}
