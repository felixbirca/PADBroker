using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using PADBroker.Sdk;

namespace PADBroker;

public class SendMessageHandler : IBrokerCommandHandler<SendMessageRequest>
{
    private ConcurrentDictionary<string, ConcurrentQueue<string>> _queues;

    public SendMessageHandler(ConcurrentDictionary<string, ConcurrentQueue<string>> queues)
    {
        _queues = queues;
    }

    public byte[] Handle(SendMessageRequest message)
    {
        var queue = _queues.GetOrAdd(message.Topic, new ConcurrentQueue<string>());

        queue.Enqueue(message.Content);

        return Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(new SendMessageResponse { Success = true })
        );
    }
}
