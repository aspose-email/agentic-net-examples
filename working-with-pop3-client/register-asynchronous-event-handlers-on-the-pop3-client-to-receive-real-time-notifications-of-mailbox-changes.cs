using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize POP3 client with placeholder credentials
            using (Pop3Client client = new Pop3Client("pop.example.com", 110, "user", "pass", SecurityOptions.Auto))
            {
                // Register event handler for successful connection
                client.OnConnect += (sender, e) =>
                {
                    Console.WriteLine("POP3 client connected.");
                };

                // Register BindIPEndPointHandler to control local endpoint selection

                // Validate credentials to trigger connection establishment
                try
                {
                    client.ValidateCredentials();
                    Console.WriteLine("Credentials validated.");
                }
                catch (Pop3Exception ex)
                {
                    Console.Error.WriteLine($"POP3 error: {ex.Message}");
                    return;
                }

                // Start a background task that periodically sends NOOP to keep the connection alive
                using (CancellationTokenSource cts = new CancellationTokenSource())
                {
                    Task keepAliveTask = Task.Run(async () =>
                    {
                        while (!cts.Token.IsCancellationRequested)
                        {
                            try
                            {
                                await client.NoopAsync(cts.Token);
                                Console.WriteLine("NOOP sent.");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"NOOP failed: {ex.Message}");
                                break;
                            }

                            await Task.Delay(TimeSpan.FromMinutes(5), cts.Token);
                        }
                    }, cts.Token);

                    // Wait for user input to stop the sample
                    Console.WriteLine("Press any key to stop...");
                    Console.ReadKey();

                    // Cancel the background task and wait for it to finish
                    cts.Cancel();
                    keepAliveTask.Wait();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
