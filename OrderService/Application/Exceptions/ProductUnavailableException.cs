namespace OrderService.Application.Exceptions
{
    public class ProductUnavailableException : Exception
    {
        public ProductUnavailableException(string message) : base(message) { }
    }
}
