using MVCQLCHTHUOC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace MVCQLCHTHUOC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index([FromQuery] string filterOn = null, string filterQuery = null, string sortBy = null, bool isAscending = true)
        {
            var jwtToken = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(jwtToken))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                var httpResponse = await client.GetAsync($"https://localhost:7220/api/Customer/get-all-customer?filterOn={filterOn}&filterQuery={filterQuery}&sortBy={sortBy}&isAscending={isAscending}");
                httpResponse.EnsureSuccessStatusCode();

                var customers = await httpResponse.Content.ReadFromJsonAsync<List<CustomerDTO>>();
                return View(customers);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<CustomerDTO>());
            }
        }

        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(CustomerAddDTO customerAddDTO)
        {
            try
            {
                var jwtToken = HttpContext.Session.GetString("JwtToken");
                if (string.IsNullOrEmpty(jwtToken))
                {
                    return RedirectToAction("Login", "Account");
                }

                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                var jsonContent = JsonConvert.SerializeObject(customerAddDTO);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json);

                var httpResponse = await client.PostAsync("https://localhost:7220/api/Customer/add-customer", httpContent);
                if (httpResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorResponse = await httpResponse.Content.ReadAsStringAsync();
                    ViewBag.Error = $"Failed to add customer: {errorResponse}";
                    return View(customerAddDTO);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"An error occurred while adding customer: {ex.Message}";
                return View(customerAddDTO);
            }
        }

        public async Task<IActionResult> ListCustomer(int id)
        {
            CustomerDTO response = new CustomerDTO();
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMess = await client.GetAsync($"https://localhost:7220/api/Customer/get-by-id/{id}");
                httpResponseMess.EnsureSuccessStatusCode();
                response = await httpResponseMess.Content.ReadFromJsonAsync<CustomerDTO>();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> EditCustomer(int id)
        {
            var jwtToken = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(jwtToken))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                var httpResponse = await client.GetAsync($"https://localhost:7220/api/Customer/get-by-id/{id}");
                httpResponse.EnsureSuccessStatusCode();

                var customer = await httpResponse.Content.ReadFromJsonAsync<CustomerDTO>();
                return View(customer);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new CustomerDTO());
            }
        }

        // POST: Customer/EditCustomer
        [HttpPost]
        public async Task<IActionResult> EditCustomer(CustomerDTO customer)
        {
            var jwtToken = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(jwtToken))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                var jsonContent = System.Text.Json.JsonSerializer.Serialize(customer);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var httpResponse = await client.PutAsync($"https://localhost:7220/api/Customer/update-by-id/{customer.Id}", httpContent);
                httpResponse.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(customer);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomerConfirmed(int id)
        {
            var jwtToken = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(jwtToken))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                var httpResponseMess = await client.DeleteAsync($"https://localhost:7220/api/Customer/del-by-id/{id}");
                httpResponseMess.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Index");
            }
        }
    }
}
