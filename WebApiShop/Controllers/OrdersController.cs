using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Text.Json;

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
        public async Task<ActionResult<OrderDetailsDTO>> GetByIdAsync([FromBody] int id)
        {
            OrderDetailsDTO order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NoContent();
            }
            return Ok(order);
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<ActionResult<OrderDetailsDTO>> AddOrderAsync([FromBody] OrderSummaryDTO dto)
        {
            OrderDetailsDTO newOrder = await _orderService.AddOrderAsync(dto);
            if (newOrder != null)
            {
                return CreatedAtAction(nameof(GetByIdAsync), new { id = newOrder.OrderId }, newOrder);
            }
            return BadRequest("Already in use by another order.");
        }

        // PUT api/<OrdersController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateStatusAsync([FromBody] OrderSummaryDTO dto)
        {
            await _orderService.UpdateStatusAsync(dto);
            return Ok();
        }

        // POST api/<OrdersController>/5/review   
        [HttpPost("{orderId}/review")]
        public async Task<ActionResult<ReviewDTO>> AddReviewAsync(int orderId, AddReviewDTO dto)
        {
            ReviewDTO newReview = await _orderService.AddReviewAsync(orderId, dto);
            if (newReview != null)
            {
                return Ok(newReview);
            }
            return BadRequest("Can't create new review");
        }

        // GET api/<OrdersController>/5/review
        [HttpGet("{orderId}/review")]
        public async Task<ActionResult<ReviewDTO>> GetReviewByOrderIdAsync([FromBody] int orderId)
        {
            ReviewDTO review = await _orderService.GetReviewByOrderIdAsync(orderId);
            if (review == null)
            {
                return NotFound($"Review for Order ID {orderId} not found");
            }
            return Ok(review);
        }

        // PUT api/<OrdersController>/5/review
        [HttpPut("{orderId}/review")]
        public async Task<IActionResult> UpdateReviewAsync([FromBody] ReviewDTO dto)
        {
            var existingReview = await _orderService.GetReviewByOrderIdAsync(dto.OrderId);
            if (existingReview == null)
            {
                return NotFound($"Review for Order ID {dto.OrderId} not found");
            }
            await _orderService.UpdateReviewAsync(dto);
            return Ok();
        }

        // GET api/<OrdersController>/5/items
        [HttpGet("{orderId}/orderItems")]
        public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetOrderItemsAsync([FromBody] int orderId)
        {
            var orderItems = await _orderService.GetOrderItemsAsync(orderId);
            if (orderItems == null || !orderItems.Any())
            {
                return NotFound($"No order items found for Order ID {orderId}");
            }
            return Ok(orderItems);
        }
    }
}
