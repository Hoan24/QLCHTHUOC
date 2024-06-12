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
    public class CustomerController : ControllerBase
    {

        private readonly ICustomer _customer;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        public CustomerController(ICustomer customer, UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _customer = customer;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }
        [Authorize(Roles = "Write")]
        [HttpGet("get-all-customer")]
        public IActionResult GetAll(string? filterOn = null, string?
filterQuery = null, string? sortBy = null,
 bool isAscending = true)
        {
            try
            {
                return Ok(_customer.GetAll(filterOn,filterQuery,sortBy,isAscending));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("/get-by-id/{id}")]
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
        [Authorize(Roles = "Write")]
        [HttpPost("add-customer")]
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
        [Authorize(Roles = "Write")]
        [HttpPut("update-by-id/{id}")]
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
        [Authorize(Roles = "Write")]
        [HttpDelete("del-by-id/{id}")]
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
