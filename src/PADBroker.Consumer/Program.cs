using PADBroker.Sdk;

public class Program
{
    private static readonly int InitialDelayMs = 100;
    private static readonly int MaxDelayMs = 5000;
    private static readonly double BackoffMultiplier = 1.5;

    public Program() { }

    public static async Task Main(string[] args)
    {
        var brokerClient = new BrokerClient(9225);

        Console.WriteLine("PAD Broker Consumer");
        Console.WriteLine("Listening for messages from 'pad-lab' topic...");
        Console.WriteLine("Press Ctrl+C to stop.");
        Console.WriteLine();

        int currentDelayMs = InitialDelayMs;

        while (true)
        {
            try
            {
                var response = await brokerClient.GetMessage("pad-lab");

                if (response != null && response.Success && !string.IsNullOrEmpty(response.Content))
                {
                    Console.WriteLine(
                        $"[{DateTime.Now:HH:mm:ss}] Received message: {response.Content}");

                    // Reset delay on successful message retrieval
                    currentDelayMs = InitialDelayMs;
                }
                else
                {
                    // No message available, apply exponential backoff
                    Console.WriteLine(
                        $"[{DateTime.Now:HH:mm:ss}] No messages available, waiting {currentDelayMs}ms...");
                    await Task.Delay(currentDelayMs);

                    // Increase delay for next iteration (exponential backoff)
                    currentDelayMs = Math.Min(
                        (int)(currentDelayMs * BackoffMultiplier),
                        MaxDelayMs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[{DateTime.Now:HH:mm:ss}] Error retrieving message: {ex.Message}");

                // Apply backoff on error as well
                await Task.Delay(currentDelayMs);
                currentDelayMs = Math.Min((int)(currentDelayMs * BackoffMultiplier), MaxDelayMs);
            }
        }
    }
}
