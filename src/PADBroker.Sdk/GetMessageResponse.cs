namespace PADBroker.Sdk;

public class GetMessageResponse : BaseResponseMessage
{
    public bool Success { get; set; }
    public string Content { get; set; } = string.Empty;
}
