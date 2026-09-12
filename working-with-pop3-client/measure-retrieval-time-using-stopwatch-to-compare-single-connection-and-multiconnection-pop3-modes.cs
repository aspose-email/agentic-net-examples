using System;
using System.Diagnostics;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace Pop3PerformanceSample
{
    class Program
    {
        static void Main()
        {
            // Top‑level exception guard
            try
            {
                // POP3 server connection settings (replace with real values)
                string host = "pop3.example.com";
                int port = 110;
                string username = "user@example.com";
                string password = "password";

                // Skip external calls when placeholder credentials are used
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                SecurityOptions security = SecurityOptions.Auto;

                // Create and configure the POP3 client
                using (Pop3Client client = new Pop3Client())
                {
                    client.Host = host;
                    client.Port = port;
                    client.Username = username;
                    client.Password = password;
                    client.SecurityOptions = security;

                    // Ensure we can connect and retrieve the message count
                    int messageCount;
                    try
                    {
                        messageCount = client.GetMessageCount();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to retrieve message count: {ex.Message}");
                        return;
                    }

                    // -------------------- Single‑connection mode --------------------
                    Stopwatch swSingle = Stopwatch.StartNew();

                    for (int i = 1; i <= messageCount; i++)
                    {
                        // Fetch each message using the default (single) connection
                        using (MailMessage message = client.FetchMessage(i))
                        {
                            // Message processing can be placed here.
                        }
                    }

                    swSingle.Stop();
                    Console.WriteLine($"Single‑connection fetch time: {swSingle.ElapsedMilliseconds} ms");

                    // -------------------- Multi‑connection mode --------------------
                    Stopwatch swMulti = Stopwatch.StartNew();

                    for (int i = 1; i <= messageCount; i++)
                    {
                        // Create an independent connection for each fetch
                        using (IConnection connection = client.CreateConnection(false))
                        {
                            using (MailMessage message = client.FetchMessage(connection, i))
                            {
                                // Message processing can be placed here.
                            }
                        }
                    }

                    swMulti.Stop();
                    Console.WriteLine($"Multi‑connection fetch time: {swMulti.ElapsedMilliseconds} ms");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
