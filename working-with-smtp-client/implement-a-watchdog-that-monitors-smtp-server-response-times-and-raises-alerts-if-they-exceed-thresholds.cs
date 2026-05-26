using System;
using System.Diagnostics;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

namespace SmtpWatchdog
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // SMTP server configuration
                string host = "smtp.example.com";
                int port = 587;
                string username = "user@example.com";
                string password = "password";
                int clientTimeoutMs = 10000; // timeout for client operations
                int responseThresholdMs = 2000; // alert threshold in milliseconds

                // Guard against placeholder credentials to avoid real network calls
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping watchdog execution.");
                    return;
                }

                // Create and configure the SMTP client
                using (SmtpClient client = new SmtpClient(host, port, username, password))
                {
                    client.Timeout = clientTimeoutMs;
                    client.GreetingTimeout = clientTimeoutMs;

                    try
                    {
                        // Measure the response time of a NOOP command
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        client.Noop();
                        stopwatch.Stop();

                        long elapsedMs = stopwatch.ElapsedMilliseconds;
                        Console.WriteLine($"SMTP NOOP response time: {elapsedMs} ms");

                        if (elapsedMs > responseThresholdMs)
                        {
                            Console.Error.WriteLine($"ALERT: SMTP response time exceeds threshold of {responseThresholdMs} ms.");
                        }
                        else
                        {
                            Console.WriteLine("SMTP response time is within acceptable limits.");
                        }
                    }
                    catch (SmtpException smtpEx)
                    {
                        Console.Error.WriteLine($"SMTP error: {smtpEx.Message}");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Unexpected error during SMTP operation: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Fatal error: {ex.Message}");
            }
        }
    }
}
