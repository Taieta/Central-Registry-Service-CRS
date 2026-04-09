using Grpc.Core;
using Grpc.Core.Interceptors;

namespace AmsGrpc.Interceptors;

public class ErrorHandlingInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception exception)
        {
            var status = new Status(StatusCode.Internal, $"Error occurred: {exception.Message}");
            var metadata = new Metadata();
            throw new RpcException(status, metadata);
        }
    }
}