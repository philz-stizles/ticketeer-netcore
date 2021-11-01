using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketeer.Application.Contracts.Infrastructure.Repositories;
using Ticketeer.Application.Contracts.Services;
using Ticketeer.Application.Models.Orders;
using Ticketeer.Domain.Entities;

namespace Ticketeer.Application.Services
{
    [Authorize]
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IUserAccessor userAccessor,
            ITicketRepository ticketRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userAccessor = userAccessor;
            _ticketRepository = ticketRepository;
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderDto>> GetAsync()
        {
            var orders = await _orderRepository.GetOrders();
            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetAsync(string id) =>
             _mapper.Map<OrderDto>((await _orderRepository.GetOrder(id)));

        public async Task<OrderDto> CreateAsync(OrderCreateDto orderCreateDto)
        {
            var newOrder = _mapper.Map<Order>(orderCreateDto);
            newOrder.CreatedDate = DateTime.Now;
            newOrder.CreatedBy = _userAccessor.GetCurrentUserId();
            var createdOrder = await _orderRepository.CreateOrder(newOrder);
            return _mapper.Map<OrderDto>(createdOrder);
        }

        public async Task UpdateAsync(string id, OrderDto orderDto) =>
            await _orderRepository.UpdateOrder(_mapper.Map<Order>(orderDto));

        /*public async Task RemoveAsync(OrderDto orderDto) =>
            await _orderRepository.DeleteOrder(_mapper.Map<Order>(orderDto));*/

        public async Task RemoveAsync(string id) =>
            await _orderRepository.DeleteOrder(id);
    }
}
