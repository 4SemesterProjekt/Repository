using ApiFormat.Group;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Services;

namespace SplitListWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private SplitListContext _context;
        private IMapper _mapper;
        private GroupService _service;

        public GroupsController(SplitListContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _service = new GroupService(context, mapper);
        }

        [HttpGet("{id}")]
        public GroupDTO Get(int id)
        {
            return _service.GetById(id);
        }

        [HttpPost]
        public GroupDTO Create(GroupDTO dto)
        {
            return _service.Create(dto);
        }

        [HttpDelete]
        public void Delete(GroupDTO dto)
        {
            _service.Delete(dto);
        }

        [HttpPut]
        public GroupDTO Update(GroupDTO dto)
        {
            return _service.Update(dto);
        }
    }
}