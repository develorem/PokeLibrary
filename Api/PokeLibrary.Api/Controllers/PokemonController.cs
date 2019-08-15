using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PokeLibrary.Api.Core;
using PokeLibrary.Api.Model;

namespace PokeLibrary.Api.Controllers
{

    [Route("api/")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly ILibraryService _libraryService;

        public PokemonController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        // GET api/library/
        [HttpGet("library")]
        public ActionResult<IEnumerable<PokemonCard>> Get()
        {
            var result = _libraryService.GetLibrary();
            return Ok(result);
        }

        // GET api/card/EX12-34
        [HttpGet("card/{id}")]
        public ActionResult<PokemonCard> Get(string id)
        {
            var result = _libraryService.Get(id);
            return Ok(result);
        }

        // DELETE api/library/EX12-34
        [HttpDelete("library/{id}")]
        public IActionResult Delete(string id)
        {
            var status = _libraryService.RemoveFromLibrary(id);
            return ProcessResult(status);
        }

        // Adds 3 instances of a card:  api/library/EX12-34/3
        [HttpPost("library/{id}/{count}")]
        public IActionResult Add([FromQuery] string id, [FromQuery] int count)
        {
            var status = _libraryService.AddToLibrary(id, count);
            return ProcessResult(status);
        }

        // Decrements that card by 3: api/library/EX12-34/3
        [HttpDelete("library/{id}/{count}")]
        public IActionResult Decrement([FromQuery] string id, [FromQuery] int count)
        {
            var status = _libraryService.AddToLibrary(id, count);
            return ProcessResult(status);
        }

        // GET api/card/EX12-34
        [HttpGet("search/{name}")]
        public ActionResult<PokemonCard> SearchName(string name)
        {
            var result = _libraryService.SearchName(name);
            return Ok(result);
        }


        private IActionResult ProcessResult(Status status)
        {
            switch (status)
            {
                case Status.Success:
                    return Ok();
                case Status.InvalidParameter:
                    return BadRequest();
                case Status.DoesNotExist:
                    return NotFound();
                default:
                    return new StatusCodeResult(500);
            }
        }
    }
}
