using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Interfaces;
using System.Security.Claims;

namespace OrderService.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<OrderDto>>> GetOrders(int page = 1, int pageSize = 20) 
        {
            var orders = await _orderService.GetPagedAsync(page, pageSize);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id) 
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create(CreateOrderDto createOrderDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            try
            {
                var created = await _orderService.CreateOrder(createOrderDto, userId);

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
