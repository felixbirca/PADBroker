using PADBroker.Sdk;

namespace PADBroker;

public interface IBrokerCommandHandler<in TRequest>
    where TRequest : BaseRequestMessage
{
    public byte[] Handle(TRequest message);
}
