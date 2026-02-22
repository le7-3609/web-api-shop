using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Text.Json;

namespace WebApiShop.Controllers
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
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetByIdAsync(int id)
        {
            OrderDetailsDTO order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NoContent();
            }
            return Ok(order);
        }

        // GET api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult<(IEnumerable<OrderDetailsDTO> Orders, double Total)>> GetOrdersAsync()
        {
            var orders = await _orderService.GetOrdersAsync();
            if (orders.Orders == null || orders.Total == 0)
            {
                return NoContent();
            }
            return Ok(orders);
        }

        // POST api/Orders/carts/5
        [HttpPost("carts/{cartId}")]
        public async Task<ActionResult<OrderDetailsDTO>> AddOrderAsync(int cartId)
        {
            try
            {
                OrderDetailsDTO newOrder = await _orderService.AddOrderFromCartAsync(cartId);
                return CreatedAtAction(nameof(GetByIdAsync), new { id = newOrder.OrderId }, newOrder);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<OrdersController>/5
        [HttpPut]
        public async Task<ActionResult> UpdateStatusAsync([FromBody] OrderSummaryDTO dto)
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
        public async Task<ActionResult> UpdateReviewAsync([FromBody] ReviewDTO dto)
        {
            var existingReview = await _orderService.GetReviewByOrderIdAsync((int)dto.OrderId);
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
