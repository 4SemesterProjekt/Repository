//using ApiFormat;
//using ApiFormat.Group;
//using ApiFormat.Pantry;
//using AutoMapper;
//using Microsoft.AspNetCore.Mvc;
//using SplitListWebApi.Areas.Identity.Data;
//using SplitListWebApi.Repositories.Implementation;
//using SplitListWebApi.Utilities;

//namespace SplitListWebApi.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class PantriesController : ControllerBase
//    {
//        private SplitListContext _context;
//        private GenericRepository<PantryDTO, PantryModel> _repository;

//        public PantriesController(SplitListContext context, IMapper mapper)
//        {
//            _context = context;
//            _repository = new GenericRepository<PantryDTO, PantryModel>(_context, mapper);
//        }

//        [HttpPost("Create")]
//        public PantryDTO Create([FromBody] PantryDTO dto)
//        {
//            return dto.Add(_repository);
//        }

//        [HttpGet("{id}")]
//        public PantryDTO GetById(int id)
//        {
//            PantryDTO dto = new PantryDTO();
//            return dto.GetById(_repository, id);
//        }

//        [HttpDelete("Delete")]
//        public void Delete([FromBody] PantryDTO dto)
//        {
//            dto.Delete(_repository);
//        }

//        [HttpPost("Save")]
//        public PantryDTO Save(PantryDTO dto)
//        {
//            return dto.Save(_repository);
//        }
//    }
//}
