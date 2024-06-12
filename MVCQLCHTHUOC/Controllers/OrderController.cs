using Microsoft.AspNetCore.Mvc;
using MVCQLCHTHUOC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using MVCQLCHTHUOC.Model.DTO;
using Microsoft.Extensions.Logging;

namespace MVCQLCHTHUOC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<OrderController> logger;

        public OrderController(IHttpClientFactory httpClientFactory, ILogger<OrderController> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
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


                var httpResponse = await client.GetAsync($"https://localhost:7220/api/Order/Get-all-Order?filterOn={filterOn}&filterQuery={filterQuery}&sortBy={sortBy}&isAscending={isAscending}");

                httpResponse.EnsureSuccessStatusCode();

                var orders = await httpResponse.Content.ReadFromJsonAsync<List<OrderDTO>>();
                return View(orders);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching orders");
                ViewBag.Error = ex.Message;
                return View(new List<OrderDTO>());
            }
        }

        [HttpGet]
        public IActionResult AddOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(OrderAddDTO orderAddDTO)
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

                var jsonContent = JsonConvert.SerializeObject(orderAddDTO);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json);

                var httpResponse = await client.PostAsync("https://localhost:7220/api/Order", httpContent);
                if (httpResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorResponse = await httpResponse.Content.ReadAsStringAsync();
                    logger.LogError("Failed to add order: {errorResponse}", errorResponse);
                    ViewBag.Error = $"Failed to add order: {errorResponse}";
                    return View(orderAddDTO);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding order");
                ViewBag.Error = $"An error occurred while adding order: {ex.Message}";
                return View(orderAddDTO);
            }
        }

      
        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id)
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

                var httpResponse = await client.DeleteAsync($"https://localhost:7220/api/Order/{id}");
                httpResponse.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the order");
                ViewBag.Error = ex.Message;
                return View("Index");
            }
        }
    }
}
