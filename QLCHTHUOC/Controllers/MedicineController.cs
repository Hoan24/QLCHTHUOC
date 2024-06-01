using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QLCHTHUOC.Model.DTO;
using QLCHTHUOC.Services.Interfaces;

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
        public MedicineController(IMedicine medicine, UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _medicine = medicine;
            _userManager= userManager;
            _tokenRepository= tokenRepository;
        }
        [AllowAnonymous]
        [HttpGet("get-all-medicine")]
        public IActionResult GetAll(string? filterOn = null, string?
filterQuery = null, string? sortBy = null,
 bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            try
            {
                return Ok(_medicine.MedicineDTOs(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageNumber));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [Authorize(Roles = "Read,Write")]
        [HttpGet("{id}")]
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
        [HttpPost]
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
        [Authorize(Roles = "Read,Write")]
        [HttpPut]
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
        [Authorize(Roles = "Read,Write")]
        [HttpDelete]
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
