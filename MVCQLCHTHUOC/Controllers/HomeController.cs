using Microsoft.AspNetCore.Mvc;
using MVCQLCHTHUOC.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System;

namespace MVCQLCHTHUOC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(string filterOn = null, string filterQuery = null, string sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 10)
        {
          

            try
            {
                var client = _httpClientFactory.CreateClient();
                var httpResponse = await client.GetAsync($"https://localhost:7220/api/Medicine/get-all-medicine?filterOn={filterOn}&filterQuery={filterQuery}&sortBy={sortBy}&isAscending={isAscending}&pageNumber={pageNumber}&pageSize={pageSize}");
                httpResponse.EnsureSuccessStatusCode();

                var medicines = await httpResponse.Content.ReadFromJsonAsync<List<MedicineDTO>>();
                return View(medicines);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<MedicineDTO>());
            }
        }

        public IActionResult Medicine()
        {
            return RedirectToAction("Index", "Medicine");
        }
    }
}
