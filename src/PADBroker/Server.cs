using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SimpleInjector;
using PADBroker.Sdk;

public class Program
{
    static Container diContainer;

    public Program() { }

    static Program()
    {
        diContainer = new Container();
        diContainer.Register(
            () => new ConcurrentDictionary<string, ConcurrentQueue<PADMessage>>(),
            Lifestyle.Singleton
        );
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

                    Console.WriteLine(Encoding.UTF8.GetString(buffer.Take(length).ToArray()));

                    if (length == 0)
                    {
                        client.Close();
                    }
                }

                Console.WriteLine("Client disconnected");
            });
        }
    }
}
