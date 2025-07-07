using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.DTOs;
using MyApp.Application.Interface;

namespace MyApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddBook(Guid userId, [FromBody] AddBookDto dto)
        {
            await _bookService.AddBookAsync(userId, dto);
            return Ok();
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserBooks(Guid userId)
        {
            var books = await _bookService.GetUserBooksAsync(userId);
            return Ok(books);
        }
    }
}
