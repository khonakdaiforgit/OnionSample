using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.DTOs;
using MyApp.Application.Interface;
using MyApp.Domain.Entities;
using MyApp.WebMVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace MyApp.WebMVC.Controllers
{
    public class BookController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public BookController(
            IHttpClientFactory httpClientFactory,
            IMapper mapper)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // خواندن توکن از کوکی
            var token = Request.Cookies["JwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var userId = GetUserIdFromToken(token);
            var response = await _httpClient.GetAsync($"book/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var books = await response.Content.ReadFromJsonAsync<List<BookDto>>();
                var viewModels = _mapper.Map<List<BookViewModel>>(books);
                return View(viewModels);
            }

            return View(new List<BookDto>());
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(AddBookViewModel model)
        {
            var token = Request.Cookies["JwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var dto = new AddBookDto { Title = model.Title, Publisher = model.Publisher };
            var userId = GetUserIdFromToken(token);

            var response = await _httpClient.PostAsJsonAsync($"book/{userId}", dto);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to add book.");
            return View(model);
        }

        private string GetUserIdFromToken(string token)
        {
            // Remove "Bearer " prefix if present
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token.Substring("Bearer ".Length);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name) ??
                                              jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name");
            return userIdClaim?.Value;
        }
    }
}
