namespace PADBroker.Sdk;

public class GetMessageRequest : BaseRequestMessage
{
    public string Topic { get; set; } = string.Empty;
}
