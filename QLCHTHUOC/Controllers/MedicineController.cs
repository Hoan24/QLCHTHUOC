using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QLCHTHUOC.Model.DTO;
using QLCHTHUOC.Services.Interfaces;
using System.Text.Json;

namespace QLCHTHUOC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicine _medicine;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly ILogger<MedicineController> _logger;
        public MedicineController(IMedicine medicine, UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, ILogger<MedicineController> logger)
        {
            _medicine = medicine;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _logger = logger;
        }

        [HttpGet("get-all-medicine")]
        [AllowAnonymous]
        public IActionResult GetAll(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true)
        {
            try
            {
                _logger.LogInformation("GetAll Medicine Action method was invoked");
                var allMedidcine = _medicine.MedicineDTOs(filterOn, filterQuery, sortBy, isAscending);
                _logger.LogInformation($"Finished GetAll request with data {JsonSerializer.Serialize(allMedidcine)}");
                return Ok(allMedidcine);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [Authorize(Roles = "Read,Write")]
        [HttpGet("get-by-id/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_medicine.GetMedicine(id));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
       
        [Authorize(Roles = "Read,Write")]
        [HttpPost("add-medicine")]
        public IActionResult AddMedicine(MedicineAddDTO medicineDTO)
        {
            try
            {
                return Ok(_medicine.MedicineAdd(medicineDTO));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Write")]
        [HttpPut("update-by-id/{id}")]
        public IActionResult UpdateCustomer(MedicineDTO medicineDTO)
        {
            try
            {
                _medicine.Update(medicineDTO);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Write")]
        [HttpDelete("del-by-id/{id}")]
        public IActionResult DeleteMedicine(int id)
        {
            try
            {
                _medicine.DeleteMedicine(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
