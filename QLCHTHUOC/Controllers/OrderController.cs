using Microsoft.AspNetCore.Mvc;
using QLCHTHUOC.Model.DTO;
using QLCHTHUOC.Model;
using QLCHTHUOC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace QLCHTHUOC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrder _order;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        public OrderController(IOrder order, UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _order = order;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }
        [Authorize(Roles = "Read,Write")]
        [HttpGet("Get-all-Order")]
        public IActionResult GetAll(string? filterOn = null, string?
filterQuery = null, string? sortBy = null,
 bool isAscending = true)
        {
            try
            {
                return Ok(_order.OrderDTOs(filterOn, filterQuery, sortBy, isAscending));
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
                return Ok(_order.GetOrder(id));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [Authorize(Roles = "Read,Write")]
        [HttpPost]
        public IActionResult Addorder(OrderAddDTO orderdto)
        {
            try
            {
                return Ok(_order.OrderAdd(orderdto));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [Authorize(Roles = "Read,Write")]
        [HttpPut]
        public IActionResult Updateorder(OrderDTO orderDTO)
        {
            try
            {
                _order.Update(orderDTO);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [Authorize(Roles = "Write")]
        [HttpDelete("{id}")]
        public IActionResult Deleteorder(int id)
        {
            try
            {
                _order.DeleteOrder(id);
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
