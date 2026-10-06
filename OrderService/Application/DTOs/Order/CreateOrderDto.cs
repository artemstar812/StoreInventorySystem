namespace OrderService.Application.DTOs.Order
{
    public class CreateOrderDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int MoneyToPay { get; set; }
    }
}
