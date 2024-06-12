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
                var registerRequest = new RegisterViewModel
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
                    ViewBag.RedirectTime = 3000;
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Registration failed.");
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
                        // Đặt giá trị vào session
                        HttpContext.Session.SetString("JwtToken", loginResponse.JwtToken);
                        HttpContext.Session.SetString("Username", loginResponse.Username);

                        // Chuyển hướng đến trang "Medicine" khi có JWT token
                        return RedirectToAction("Index", "Medicine");
                    }
                    else
                    {
                        // Xử lý lỗi khi không nhận được JWT token
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
