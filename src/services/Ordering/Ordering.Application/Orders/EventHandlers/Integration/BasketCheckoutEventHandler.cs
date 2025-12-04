using BuildingBlocks.Messaging.Events;
using MassTransit;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.EventHandlers.Integration
{
    public class BasketCheckoutEventHandler 
        (ISender sender, ILogger<BasketCheckoutEventHandler> logger)
        : IConsumer<BasketCheckoutEvent>
    {
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            //TODO: Create order and start fullfillment process
            logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);
            var command = MapToCreateOrderCommand(context.Message);
            await sender.Send(command);
        }

        private static CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
        {
            var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, 
                message.AddressLine, message.Country, message.State, message.ZipCode);
            var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.Cvv, message.PaymentMethod);
            var orderId = Guid.NewGuid();

            var orderDto = new OrderDto(
                   Id: orderId,
                   CustomerId: message.CustomerId,
                   OrderName: message.UserName,
                   ShippingAddress: addressDto,
                   BillingAddress: addressDto,
                   Payment: paymentDto,
                   Status: Ordering.Domain.Enums.OrderStatus.Pending,
                   OrderItems: [
                        new OrderItemDto(orderId, new Guid("e6dde822-c848-4f07-82db-700a646b39cd"), 1, 120),
                        new OrderItemDto(orderId, new Guid("48dbbb8c-bcef-4e42-b39a-dbc2e2dc4417"), 2, 30)
                       ]
                );

            return new CreateOrderCommand(orderDto);
        }
    }
}  