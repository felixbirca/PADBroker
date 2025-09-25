using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

public class GetMessageHandler : IPADCommandHandler
{
    private ConcurrentDictionary<string, ConcurrentQueue<PADMessage>> _queues;

    public GetMessageHandler(ConcurrentDictionary<string, ConcurrentQueue<PADMessage>> queues)
    {
        _queues = queues;
    }

    public byte[] Handle(byte[] message)
    {
        var messageObj = JsonSerializer.Deserialize<GetMessageRequest>(
            Encoding.UTF8.GetString(message)
        );

        if (!_queues.TryGetValue(messageObj.QueueName, out var queue))
        {
            return Encoding.UTF8.GetBytes("Queue does not exist.");
        }

        if (queue.TryDequeue(out var responseMessage))
        {
            return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(responseMessage));
        }

        return Encoding.UTF8.GetBytes("Queue is empty.");
    }
}
