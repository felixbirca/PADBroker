using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace PADBroker.Sdk;

public class BrokerClient
{
    private int _port { get; set; }

    public BrokerClient(int port)
    {
        _port = port;
    }

    public async Task SendMessage(string topic, string content)
    {
        var tcpClient = new TcpClient();

        await tcpClient.ConnectAsync("localhost", _port);

        var stream = tcpClient.GetStream();

        await stream.WriteAsync(
            Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize<BaseRequestMessage>(
                    new SendMessageRequest { Topic = topic, Content = content }
                )
            )
        );
        await stream.FlushAsync();

        var buffer = new byte[1024];
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

        if (bytesRead == 0)
        {
            // Server closed connection or sent nothing
            Console.WriteLine("No response from server");
            return;
        }

        Console.WriteLine("response string ", Encoding.UTF8.GetString(buffer, 0, bytesRead));

        var response =
            JsonSerializer.Deserialize<BaseResponseMessage>(
                Encoding.UTF8.GetString(buffer, 0, bytesRead)
            ) as SendMessageResponse;

        if (!response.Success)
        {
            throw new Exception("Something went wrong");
        }
    }

    public async Task<GetMessageResponse> GetMessage(string topic)
    {
        var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync("localhost", _port);

        var stream = tcpClient.GetStream();

        await stream.WriteAsync(
            Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize<BaseRequestMessage>(
                    new GetMessageRequest { Topic = topic }
                )
            )
        );
        await stream.FlushAsync();

        var buffer = new byte[1024];
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

        if (bytesRead == 0)
        {
            // Server closed connection or sent nothing
            throw new Exception("Server closed connection");
        }

        var response = JsonSerializer.Deserialize<GetMessageResponse>(
            Encoding.UTF8.GetString(buffer, 0, bytesRead)
        );

        if (response == null)
        {
            throw new Exception("Failed to deserialize response");
        }

        return response;
    }
}
