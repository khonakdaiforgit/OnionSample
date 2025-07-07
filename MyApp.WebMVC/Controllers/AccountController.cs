using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.DTOs;
using MyApp.Domain.Entities;
using MyApp.WebMVC.Models;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyApp.WebMVC.Controllers
{
    // WebMvc/Controllers/AccountController.cs
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebApi");
        }

        public IActionResult Profile()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var dto = new RegisterUserDto
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password
            };

            var response = await _httpClient.PostAsJsonAsync("user/register", dto);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Registration failed.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var dto = new Application.DTOs.LoginDto { Username = model.Username, Password = model.Password };
            var response = await _httpClient.PostAsJsonAsync("user/login", dto);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadFromJsonAsync<LoginResponse>();
                var token = responseContent.Token;
                Console.WriteLine($"Storing Token in Cookie: {token}");

                // Store JWT token for Web API calls
                Response.Cookies.Append("JwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddHours(1)
                });

                // Create claims for cookie authentication
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username), // Username for MVC
                    new Claim(ClaimTypes.NameIdentifier, GetUserIdFromToken(token)) // User ID
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(1)
                };

                // Sign in the user with cookie authentication
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Book");
            }

            ModelState.AddModelError("", "Login failed.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Clear authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Clear JWT token cookie
            Response.Cookies.Delete("JwtToken");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private string GetUserIdFromToken(string token)
        {
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = token.Substring("Bearer ".Length);
            }

            if (string.IsNullOrEmpty(token) || token.Split('.').Length != 3)
            {
                Console.WriteLine("Invalid JWT token format.");
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var jwtToken = tokenHandler.ReadJwtToken(token);
                Console.WriteLine("Token Claims:");
                foreach (var claim in jwtToken.Claims)
                {
                    Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
                }

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name) ??
                                  jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name");
                if (userIdClaim == null)
                {
                    Console.WriteLine("Claim 'unique_name' or 'Name' not found.");
                    return null;
                }

                return userIdClaim.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing JWT: {ex.Message}");
                return null;
            }
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}

