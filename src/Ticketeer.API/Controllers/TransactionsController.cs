/*using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.API.Attributes;
using Ticketeer.Application.Contracts.Services;
using Ticketeer.Application.Models.Transactions;
using Ticketeer.Domain.Enums;

namespace Ticketeer.API.Controllers
{
    [Authorize(RoleType.Vendor)]
    public class TransactionsController : BaseController
    {
        private readonly ITransactionService _ticketService;
        public TransactionsController(ITransactionService ticketService)
        {
            _ticketService = ticketService;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TransactionDto>))]
        public async Task<ActionResult<List<TransactionDto>>> Get()
        {
            var tickets = await _ticketService.GetAsync();
            return Ok(tickets);
        }

        [AllowAnonymous]
        [HttpGet("{id:length(24)}", Name = "GetTransaction")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransactionDto))]
        public async Task<ActionResult<TransactionDto>> Get(string id)
        {
            var book = await _ticketService.GetAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
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
*/