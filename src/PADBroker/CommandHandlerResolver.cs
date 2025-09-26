using PADBroker.Sdk;
using SimpleInjector;

namespace PADBroker;

public class CommandHandlerResolver
{
    private Container _container;

    public CommandHandlerResolver(Container container)
    {
        _container = container;
    }

    public byte[] Resolve(BaseRequestMessage request)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IBrokerCommandHandler<>).MakeGenericType(requestType);
        var handler = _container.GetInstance(handlerType);

        if (handler is null)
            throw new InvalidOperationException("No handler for this message type");

        var method = handlerType.GetMethod("Handle");
        var response = method!.Invoke(handler, new object[] { request });
        return response as byte[]
            ?? Array.Empty<byte[]>().SelectMany(_ => Array.Empty<byte>()).ToArray();
    }
}
