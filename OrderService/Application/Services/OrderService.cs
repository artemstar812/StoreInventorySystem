using InventoryService.Grpc;
using OrderService.Application.DTOs;
using OrderService.Application.DTOs.Order;
using OrderService.Application.Exceptions;
using OrderService.Application.Interfaces;
using OrderService.Application.Mapppers;
using OrderService.Domain.Entity;
using Shared.Contracts.Events;
using static InventoryService.Grpc.InventoryGrpc;

namespace OrderService.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly InventoryGrpcClient _inventoryGrpcClient;
        private readonly IEventProducer _eventProducer;

        public OrderService(IOrderRepository repository, InventoryGrpcClient grpcClient, IEventProducer eventProducer)
        {
            _repository = repository;
            _inventoryGrpcClient = grpcClient;
            _eventProducer = eventProducer;
        }

        public async Task<OrderDto> CreateOrder(CreateOrderDto dto, int userId)
        {
            var response = await _inventoryGrpcClient.ReserveProductAsync(new ReserveProductRequest
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            });

            if(!response.Success)
            {
                /*throw new ProductUnavailableException(response.Message);*/
                await ThrowOrderFailedAsync(response.Message, new ProductUnavailableException(response.Message), dto.ProductId, dto.Quantity);
            }

            if(response.Price > dto.MoneyToPay)
            {
                await ThrowOrderFailedAsync("Insufficient Funds", new InsufficientFundsException(), dto.ProductId, dto.Quantity);
            }

            var order = new Order
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                Status = Domain.Enums.OrderStatus.Pending,
                TotalPrice = (decimal)response.Price,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(order);
            await ThrowOrderCreatedAsync(order);

            return OrderMapper.ToDto(order);
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _repository.GetByIdAsync(id);

            if (order != null)
                return OrderMapper.ToDto(order);
            else
                return null;
        }

        public async Task<PagedResult<OrderDto>> GetPagedAsync(int page, int pageSize)
        {
            var (orders, total) = await _repository.GetPagedAsync(page, pageSize);

            var ordersDto = new List<OrderDto>();

            foreach(var order in orders)
            {
                var orderDto = OrderMapper.ToDto(order);

                ordersDto.Add(orderDto);
            }

            var results = new PagedResult<OrderDto>
            {
                Items = ordersDto,
                TotalCount = total
            };

            return results;
        }

        private async Task ThrowOrderFailedAsync(string reason, Exception exception, int productId, int quantity)
        {
            await _eventProducer.PublishAsync(new OrderFailedEvent
            {
                EventId = Guid.NewGuid(),
                ProductId = productId,
                Quantity = quantity,
                Reason = reason
            });

            throw exception;
        }

        private async Task ThrowOrderCreatedAsync(Order created) 
        {
            await _eventProducer.PublishAsync(new OrderCreatedEvent
            {
                EventId = Guid.NewGuid(),
                OrderId = created.Id,
                ProductId = created.ProductId,
                Quantity = created.Quantity,
                TotalPrice = created.TotalPrice
            });
        }
    }
}
