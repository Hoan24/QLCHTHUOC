using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MVCQLCHTHUOC.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace MVCQLCHTHUOC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
       
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var registerRequest = new RegisterRequestDTO
                {
                    Username = model.Username,
                    Password = model.Password,
                    Roles = model.Roles
                };

                var client = _httpClientFactory.CreateClient();
                var jsonContent = JsonConvert.SerializeObject(registerRequest);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7220/api/User/Register", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.RegisterSuccess = true;
                    return RedirectToAction("Login");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Registration failed: {errorContent}");
                }
            }
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
            if (ModelState.IsValid)
            {
                var loginRequest = new LoginViewModel
                {
                    Username = model.Username,
                    Password = model.Password
                };

                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("https://localhost:7220/api/User/Login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponseDTO>(jsonResponse);

                    
                    if (!string.IsNullOrEmpty(loginResponse.JwtToken))
                    {
                        HttpContext.Session.SetString("JwtToken", loginResponse.JwtToken);
                        HttpContext.Session.SetString("Username", loginResponse.Username);

                        return RedirectToAction("Index", "Medicine");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No JWT token received from the server.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Login failed. Please check your credentials.");
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Xóa token
            HttpContext.Session.Remove("JwtToken");
            return RedirectToAction("Index", "Home");
        }
    }
}
