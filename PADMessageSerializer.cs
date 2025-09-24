using System.Text.Json;

public static class PADMessageSerializer
{
    public static async Task<string> SerializeAsync()
    {

    }

    public static async Task<PADMessage> DeserializeAsync(string message)
    {
        var obj = JsonSerializer.Deserialize<PADMessage>(message);

        if (obj == null)
        {
            throw new Exception("Empty message");
        }

        return obj;
    }
}