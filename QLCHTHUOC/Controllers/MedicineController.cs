using Microsoft.AspNetCore.Mvc;
using QLCHTHUOC.Model.DTO;
using QLCHTHUOC.Services.Interfaces;

namespace QLCHTHUOC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicine _medicine;
        public MedicineController(IMedicine medicine)
        {
            _medicine = medicine;
        }
        
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
