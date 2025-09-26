using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using PADBroker.Sdk;

namespace PADBroker;

public class GetMessageHandler : IBrokerCommandHandler<GetMessageRequest>
{
    private ConcurrentDictionary<string, ConcurrentQueue<string>> _queues;

    public GetMessageHandler(ConcurrentDictionary<string, ConcurrentQueue<string>> queues)
    {
        _queues = queues;
    }

    public byte[] Handle(GetMessageRequest message)
    {
        if (!_queues.TryGetValue(message.QueueName, out var queue))
        {
            return Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(new GetMessageResponse { Success = false })
            );
        }

        if (queue.TryDequeue(out var messageContent))
        {
            return Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(
                    new GetMessageResponse { Success = true, Content = messageContent }
                )
            );
        }

        return Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(new GetMessageResponse { Success = false })
        );
    }
}
