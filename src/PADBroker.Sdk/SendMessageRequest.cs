namespace PADBroker.Sdk;

public class SendMessageRequest : BaseRequestMessage
{
    public string Topic { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
