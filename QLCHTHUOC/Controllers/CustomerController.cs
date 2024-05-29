using Microsoft.AspNetCore.Mvc;
using QLCHTHUOC.Model.DTO;
using QLCHTHUOC.Services.Interfaces;

namespace QLCHTHUOC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomer _customer;
        public CustomerController(ICustomer customer)
        {
            _customer = customer;
        }
        [HttpGet("get-all-customer")]
        public IActionResult GetAll(string? filterOn = null, string?
filterQuery = null, string? sortBy = null,
 bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            try
            {
                return Ok(_customer.GetAll(filterOn,filterQuery,sortBy,isAscending,pageNumber,pageNumber));
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
                return Ok(_customer.GetCustomer(id));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        public IActionResult AddCustomer(CustomerAddDTO customerDTO)
        {
            try
            {
                return Ok(_customer.CustomerAdd(customerDTO));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        public IActionResult UpdateCustomer(CustomerDTO customerDTO)
        {
            try
            {
                _customer.Update(customerDTO);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete]
        public IActionResult DeleteCustomer(int id)
        {
            try
            {
                _customer.DeleteCustomer(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
