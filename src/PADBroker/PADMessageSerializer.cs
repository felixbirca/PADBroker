using System.Text;
using System.Text.Json;

public static class PADMessageSerializer
{
    public static byte[] Serialize(PADMessage message)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
    }

    public static PADMessage Deserialize(byte[] message)
    {
        var obj = JsonSerializer.Deserialize<PADMessage>(message);

        if (obj == null)
        {
            throw new Exception("Empty message");
        }

        return obj;
    }
}