using Aspose.Email.Clients.Exchange;
using System;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailLogDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder connection details
                string serviceUrl = "https://example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard against executing real network calls with placeholder credentials
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external service calls.");
                    return;
                }

                // Create and use the EWS client within a using block to ensure disposal
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    try
                    {
                        // Attempt to retrieve mailbox information (wrapped in its own try/catch)
                        ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                        Console.WriteLine($"Mailbox URI: {mailboxInfo.MailboxUri}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error fetching mailbox info: {ex.Message}");
                    }

                    // Build diagnostic log output
                    StringBuilder logBuilder = new StringBuilder();
                    logBuilder.AppendLine("=== Diagnostic Log Start ===");
                    logBuilder.AppendLine($"Connected to: {serviceUrl}");
                    logBuilder.AppendLine($"User: {username}");
                    logBuilder.AppendLine("Operation completed successfully.");
                    logBuilder.AppendLine("=== Diagnostic Log End ===");

                    // Display the log in the console
                    Console.WriteLine(logBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                // Top-level exception guard
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
