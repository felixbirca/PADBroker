using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using PADBroker.Sdk;
using SimpleInjector;

namespace PADBroker;

public class Program
{
    static Container diContainer;

    public Program() { }

    static Program()
    {
        diContainer = new Container();
        diContainer.Register(
            () => new ConcurrentDictionary<string, ConcurrentQueue<string>>(),
            Lifestyle.Singleton
        );
        diContainer.Register<IBrokerCommandHandler<GetMessageRequest>, GetMessageHandler>(
            Lifestyle.Transient
        );
        diContainer.Register<IBrokerCommandHandler<SendMessageRequest>, SendMessageHandler>(
            Lifestyle.Transient
        );
        diContainer.Register<CommandHandlerResolver>();
    }

    public static async Task Main()
    {
        TcpListener server = new TcpListener(IPAddress.Any, 9225);
        server.Start();

        Console.WriteLine("Server started");
        while (true)
        {
            var client = await server.AcceptSocketAsync();

            var _ = Task.Run(async () =>
            {
                while (client.Connected)
                {
                    var buffer = new byte[1024];
                    var length = await client.ReceiveAsync(buffer);

                    if (length == 0)
                    {
                        client.Close();
                    }

                    var commandHandlerResolver = diContainer.GetInstance<CommandHandlerResolver>();

                    var messageObj = JsonSerializer.Deserialize<BaseRequestMessage>(
                        Encoding.UTF8.GetString(buffer.Take(length).ToArray())
                    );

                    var response = commandHandlerResolver.Resolve(messageObj);

                    await client.SendAsync(response);
                }

                Console.WriteLine("Client disconnected");
            });
        }
    }
}
