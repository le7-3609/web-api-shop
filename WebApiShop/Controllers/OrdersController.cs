using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entities;
using Services;

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _iorderService;

        public OrdersController(IOrderService orderService)
        {
            _iorderService = orderService;
        }

        // GET api/<OrdersController>/5
        [ActionName("GetByIdAsync")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetByIdAsync(int id)
        {
            Order order = await _iorderService.GetByIdAsync(id);
            if (order == null)
            {
                return NoContent();
            }
            return Ok(order);
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<ActionResult<Order>> AddOrderAsync([FromBody] Order order)
        {
            Order newOrder = await _iorderService.AddOrderAsync(order);
            if (newOrder != null)
            {
                return CreatedAtAction(nameof(GetByIdAsync), new { id = newOrder.OrderId }, newOrder);
            }
            return BadRequest("Already in use by another order.");
        }
    }
}
