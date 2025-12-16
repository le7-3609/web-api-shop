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
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET api/<OrdersController>/5
        [ActionName("GetByIdAsync")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetByIdAsync(int id)
        {
            Order order = await _orderService.GetByIdAsync(id);
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
            Order newOrder = await _orderService.AddOrderAsync(order);
            if (newOrder != null)
            {
                return CreatedAtAction(nameof(GetByIdAsync), new { id = newOrder.OrderId }, newOrder);
            }
            return BadRequest("Already in use by another order.");
        }
    }
}
