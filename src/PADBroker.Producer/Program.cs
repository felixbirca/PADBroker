using PADBroker.Sdk;

namespace PADBroker.Producer;

public class Program
{
    public Program() { }

    public static async Task Main(string[] args)
    {
        var brokerClient = new BrokerClient(9225);

        Console.WriteLine("PAD Broker Producer");
        Console.WriteLine("Type messages to send to 'pad-lab' topic. Type 'exit' to quit.");
        Console.WriteLine("Press Enter after each message to send it.");
        Console.WriteLine();

        string? input;
        do
        {
            Console.Write("Enter message: ");
            input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input) && input.ToLower() != "exit")
            {
                try
                {
                    await brokerClient.SendMessage("pad-lab", input);
                    Console.WriteLine($"Message sent successfully: {input}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending message: {ex.Message}");
                }
            }
        } while (input?.ToLower() != "exit");

        Console.WriteLine("Goodbye!");
    }
}
