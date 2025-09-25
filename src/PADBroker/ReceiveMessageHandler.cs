using System.Collections.Concurrent;
using System.Text;

public class ReceiveMessageHandler
{
    private ConcurrentDictionary<string, ConcurrentQueue<PADMessage>> _queues;

    public ReceiveMessageHandler(ConcurrentDictionary<string, ConcurrentQueue<PADMessage>> queues)
    {
        _queues = queues;
    }

    public byte[] Handle(byte[] message)
    {
        var messageObj = PADMessageSerializer.Deserialize(message);

        var queue = _queues.GetOrAdd(messageObj.Topic, new ConcurrentQueue<PADMessage>());

        queue.Enqueue(messageObj);

        return Encoding.UTF8.GetBytes("Ok");
    }
}