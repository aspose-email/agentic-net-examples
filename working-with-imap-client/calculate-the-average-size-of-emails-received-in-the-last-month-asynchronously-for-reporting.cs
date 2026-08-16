using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Guard against executing real network calls with placeholder data
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Create and configure the IMAP client
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
            {
                try
                {
                    client.Username = username;
                    client.Password = password;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Authentication failed: {ex.Message}");
                    return;
                }

                // Define the time window: last month
                DateTime now = DateTime.UtcNow;
                DateTime firstDayOfCurrentMonth = new DateTime(now.Year, now.Month, 1);
                DateTime firstDayOfLastMonth = firstDayOfCurrentMonth.AddMonths(-1);
                DateTime lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);

                // Retrieve message infos from INBOX
                ImapMessageInfoCollection messageInfos;
                try
                {
                    // Retrieve all messages; filtering will be done in code
                    messageInfos = await client.ListMessagesAsync("INBOX", null, 0, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                long totalSize = 0;
                int count = 0;

                foreach (var info in messageInfos)
                {
                    // Filter by internal date (received date)
                    if (info.InternalDate >= firstDayOfLastMonth && info.InternalDate <= lastDayOfLastMonth)
                    {
                        // Prefer using the size from the message info if available
                        totalSize += info.Size;
                        count++;
                    }
                }

                if (count == 0)
                {
                    Console.WriteLine("No messages received in the last month.");
                    return;
                }

                double averageSize = (double)totalSize / count;
                Console.WriteLine($"Average size of emails received last month: {averageSize:F2} bytes ({count} messages).");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
