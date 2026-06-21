using Grpc.Core;
using Grpc.Core.Interceptors;

namespace UserService.Infrastructure.Grpc
{
    public class LoggingInterceptor : Interceptor
    {
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            Console.WriteLine($"Method: {context.Method}");

            try
            {
                var response = await continuation(request, context);

                Console.WriteLine("Success");

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                throw;
            }
        }
    }
}
