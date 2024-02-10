using Application.Abstraction.Services;
using Application.Request;
using Application.Response;
using Domain.Models;
using Domain.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookSearchController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookSearchController(IBookService bookService)
        {
            _bookService = bookService;
                
        }

        [HttpPost]
        [ProducesResponseType(typeof(ResultOfAction<PagedListResult<BookserachResponse>>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Search([FromBody] searchBookRequest  request)
        {
          var result = await  _bookService.SearchBookAsync(request);

            return Ok(result); 
        }
    }
}
    