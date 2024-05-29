using Microsoft.AspNetCore.Mvc;
using QLCHTHUOC.Model.DTO;
using QLCHTHUOC.Model;
using QLCHTHUOC.Services.Interfaces;

namespace QLCHTHUOC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrder _order;
        public OrderController(IOrder order)
        {
            _order = order;
        }
        [HttpGet("Get-all-Order")]
        public IActionResult GetAll(string? filterOn = null, string?
filterQuery = null, string? sortBy = null,
 bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            try
            {
                return Ok(_order.OrderDTOs(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageNumber));
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
                return Ok(_order.GetOrder(id));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        public IActionResult Addorder(OrderDTO orderdto)
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
        [HttpDelete]
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
