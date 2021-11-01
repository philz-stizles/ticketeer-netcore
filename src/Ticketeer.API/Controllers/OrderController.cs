using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Application.Contracts.Services;
using Ticketeer.Application.Models.Orders;

namespace Ticketeer.API.Controllers
{
    // [Authorize(RoleType.Vendor, RoleType.User)]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDto>))]
        public async Task<ActionResult<List<OrderDto>>> Get()
        {
            var orders = await _orderService.GetAsync();
            return Ok(orders);
        }

        [HttpGet("{id:length(24)}", Name = "GetOrder")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        public async Task<ActionResult<OrderDto>> Get(string id)
        {
            var book = await _orderService.GetAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDto))]
        public async Task<ActionResult<OrderDto>> Create(OrderCreateDto orderDto)
        {
            var createdOrderDto = await _orderService.CreateAsync(orderDto);

            return CreatedAtRoute("GetOrder", new { id = createdOrderDto.Id.ToString() }, createdOrderDto);
        }

        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(string id, OrderDto bookIn)
        {
            var book = _orderService.GetAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            await _orderService.UpdateAsync(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string id)
        {
            await _orderService.RemoveAsync(id);

            return NoContent();
        }
    }
}
