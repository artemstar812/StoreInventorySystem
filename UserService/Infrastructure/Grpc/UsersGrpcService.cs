using Grpc.Core;
using UserService.Application.Interfaces;
using UserService.Grpc;

namespace UserService.Infrastructure.Grpc
{
    public class UsersGrpcService : UsersGrpc.UsersGrpcBase
    {
        private readonly IUserRepository _repository;

        public UsersGrpcService(IUserRepository repository)
        {
            _repository = repository;
        }

        public override async Task<UserResponse> GetUserById(GetUserRequest request, ServerCallContext context)
        {
            var user = await _repository.GetByIdAsync(request.Id);

            if(user is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User Not Found"));
            }

            return new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };
        }
    }
}
