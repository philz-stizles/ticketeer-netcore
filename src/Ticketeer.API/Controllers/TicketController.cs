using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.API.Attributes;
using Ticketeer.Application.Contracts.Services;
using Ticketeer.Application.Models;
using Ticketeer.Domain.Enums;

namespace Ticketeer.API.Controllers
{
    [Authorize(RoleType.Vendor)]
    public class TicketController : BaseController
    {
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TicketDto>))]
        public async Task<ActionResult<List<TicketDto>>> Get()
        {
            var tickets = await _ticketService.GetAsync();
            return Ok(tickets);
        }

        [AllowAnonymous]
        [HttpGet("{id:length(24)}", Name = "GetTicket")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TicketDto))]
        public async Task<ActionResult<TicketDto>> Get(string id)
        {
            var book = await _ticketService.GetAsync(id);

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
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TicketDto))]
        public async Task<ActionResult<TicketDto>> Create(TicketCreateDto ticketDto)
        {
            var createdTicketDto = await _ticketService.CreateAsync(ticketDto);

            return CreatedAtRoute("GetTicket", new { id = createdTicketDto.Id.ToString() }, createdTicketDto);
        }

        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(string id, TicketDto bookIn)
        {
            var book = _ticketService.GetAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            await _ticketService.UpdateAsync(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string id)
        {
            await _ticketService.RemoveAsync(id);

            return NoContent();
        }
    }
}
