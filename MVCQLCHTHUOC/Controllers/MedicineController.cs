using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCQLCHTHUOC.Models;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MVCQLCHTHUOC.Controllers
{
    public class MedicineController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IWebHostEnvironment webHostEnvironment;

        public MedicineController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment)
        {
            this.httpClientFactory = httpClientFactory;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string filterOn = null, string filterQuery = null, string sortBy = null, bool isAscending = true)
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

                HttpResponseMessage httpResponse;
                if (!string.IsNullOrEmpty(filterQuery))
                {
                    httpResponse = await client.GetAsync($"https://localhost:7220/api/Medicine/get-all-medicine?filterOn=name&filterQuery={filterQuery}&sortBy={sortBy}&isAscending={isAscending}");
                }
                else
                {
                    httpResponse = await client.GetAsync($"https://localhost:7220/api/Medicine/get-all-medicine?filterOn={filterOn}&sortBy={sortBy}&isAscending={isAscending}");
                }

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


        [HttpGet]
        public IActionResult AddMedicine()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicine(MedicineAddDTO medicineAddDTO, IFormFile uploadimg)
        {
            try
            {
                var jwtToken = HttpContext.Session.GetString("JwtToken");
                if (string.IsNullOrEmpty(jwtToken))
                {
                    return RedirectToAction("Login", "Account");
                }

             
                if (uploadimg != null && uploadimg.Length > 0)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Upload/Medicine");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(uploadimg.FileName); // Sử dụng Path.GetFileName để tránh các vấn đề về đường dẫn
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);


                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadimg.CopyToAsync(fileStream);
                    }

                 
                    medicineAddDTO.ImgURl = uniqueFileName;
                }

                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                var jsonContent = JsonSerializer.Serialize(medicineAddDTO);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, MediaTypeNames.Application.Json);

                var httpResponse = await client.PostAsync("https://localhost:7220/api/Medicine/add-medicine", httpContent);
                if (httpResponse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var errorResponse = await httpResponse.Content.ReadAsStringAsync();
                    ViewBag.Error = $"Failed to add medicine: {errorResponse}";
                    return View(medicineAddDTO);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"An error occurred while adding medicine: {ex.Message}";
                return View(medicineAddDTO);
            }
        }


        public async Task<IActionResult> ListMedicinebyid(int id)
        {
            MedicineDTO response = new MedicineDTO();
            var jwtToken = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(jwtToken))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var client = httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                var httpResponseMess = await client.GetAsync($"https://localhost:7220/api/Medicine/get-by-id/{id}");
                httpResponseMess.EnsureSuccessStatusCode();
                response = await httpResponseMess.Content.ReadFromJsonAsync<MedicineDTO>();
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(response);
        }


        [HttpGet]
        public async Task<IActionResult> EditMedicine(int id)
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

                var httpResponseMess = await client.GetAsync($"https://localhost:7220/api/Medicine/get-by-id/{id}");
                httpResponseMess.EnsureSuccessStatusCode();
                var responseBook = await httpResponseMess.Content.ReadFromJsonAsync<MedicineEditDTO>();

                return View(responseBook);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditMedicine(int id, MedicineEditDTO medicineDTO)
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

                var httpRequestMess = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri($"https://localhost:7220/api/Medicine/update-by-id/{id}"),
                    Content = new StringContent(JsonSerializer.Serialize(medicineDTO), Encoding.UTF8, "application/json")
                };

                var httpResponseMess = await client.SendAsync(httpRequestMess);
                httpResponseMess.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("EditMedicine", medicineDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMedicineConfirmed(int id)
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

                var httpResponseMess = await client.DeleteAsync($"https://localhost:7220/api/Medicine/del-by-id/{id}");
                httpResponseMess.EnsureSuccessStatusCode();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View("Index");
            }
        }
        public IActionResult ListMedicine(List<MedicineDTO> medicines)
        {
            return View(medicines);
        }

    }
}
