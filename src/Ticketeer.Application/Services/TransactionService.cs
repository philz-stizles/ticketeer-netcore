using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Application.Contracts.Infrastructure.Repositories;
using Ticketeer.Application.Contracts.Services;
using Ticketeer.Application.Models;

namespace Ticketeer.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public TransactionService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task<List<TicketDto>> GetAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<TicketDto> GetAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveAsync(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
