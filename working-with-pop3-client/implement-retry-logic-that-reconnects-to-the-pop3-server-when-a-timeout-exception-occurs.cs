using System;
using System.Net;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server connection settings (placeholders)
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI
            if (host.Contains("example") || username.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder POP3 credentials detected. Skipping network operation.");
                return;
            }

            const int maxRetries = 3;
            int attempt = 0;
            bool connected = false;

            while (attempt < maxRetries && !connected)
            {
                attempt++;

                // Create a new POP3 client for each attempt
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        // Attempt to validate credentials which forces a connection
                        client.ValidateCredentials();

                        Console.WriteLine("Connected to POP3 server successfully.");
                        connected = true;

                        // Example operation: retrieve message count
                        int messageCount = client.GetMessageCount();
                        Console.WriteLine($"Total messages: {messageCount}");
                    }
                    catch (Pop3Exception ex) when (ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase) ||
                                                   ex.InnerException is System.TimeoutException)
                    {
                        Console.Error.WriteLine($"Timeout occurred on attempt {attempt}: {ex.Message}");

                        if (attempt < maxRetries)
                        {
                            Console.WriteLine("Retrying connection...");
                            // Optional: wait before retrying
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            Console.Error.WriteLine("Maximum retry attempts reached. Unable to connect.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle other exceptions without retry
                        Console.Error.WriteLine($"An error occurred: {ex.Message}");
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
