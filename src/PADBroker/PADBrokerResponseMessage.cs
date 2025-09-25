public class PADBrokerResponseMessage
{
    public PADBrokerResponseMessageStatus Status { get; set; }
    public string Content { get; set; }
}

public enum PADBrokerResponseMessageStatus
{
    Ok,
    Error,
    CriticalError,
}
