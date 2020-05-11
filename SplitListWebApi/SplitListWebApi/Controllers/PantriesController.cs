using ApiFormat;
using ApiFormat.Group;
using ApiFormat.Pantry;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Services;
using SplitListWebApi.Services.Interfaces;
using SplitListWebApi.Utilities;
using System;
using System.Threading.Tasks;

namespace SplitListWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PantriesController : ControllerBase
    {
        private SplitListContext _context;
        private PantryService pantryService;
        

        public PantriesController(SplitListContext context, IMapper mapper)
        {
            _context = context;
            pantryService = new PantryService(context, mapper);
        }

        [HttpGet("{id}")]
        public PantryDTO GetById(int id)
        {
            return pantryService.GetById(id);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] PantryDTO dto)
        {
            try
            {
                pantryService.Delete(dto);
                return Ok(dto);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
            
        }

        [HttpPut]
        public ActionResult<PantryDTO> Update(PantryDTO dto)
        {
            try
            {
                return pantryService.Update(dto);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
