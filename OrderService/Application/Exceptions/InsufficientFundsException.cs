namespace OrderService.Application.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException() : base("Insufficient funds.")
        { }
    }
}
