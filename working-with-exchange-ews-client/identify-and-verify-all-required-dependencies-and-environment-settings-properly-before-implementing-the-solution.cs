using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Verify and load Aspose.Email license if present
            string licensePath = "Aspose.Email.lic";
            if (File.Exists(licensePath))
            {
                try
                {
                    using (FileStream licStream = File.OpenRead(licensePath))
                    {
                        License license = new License();
                        license.SetLicense(licStream);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load license: {ex.Message}");
                    return;
                }
            }

            // Verify sample EML file exists before loading
            string emlPath = "sample.eml";
            if (File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage message = MailMessage.Load(emlPath))
                    {
                        Console.WriteLine($"Loaded message subject: {message.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                    return;
                }
            }
            else
            {
                Console.Error.WriteLine("EML file not found; skipping load step.");
            }

            // Initialize EWS client safely
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(
                    "https://example.com/EWS/Exchange.asmx",
                    "username",
                    "password");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Use the client within a using block to ensure disposal
            using (client)
            {
                // Build a simple query to filter messages
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.Subject.Contains("Test");
                MailQuery query = builder.GetQuery();

                // List messages in the Inbox matching the query
                try
                {
                    var messageInfos = client.ListMessages(client.MailboxInfo.InboxUri, query, false);
                    foreach (var info in messageInfos)
                    {
                        // Fetch each full message
                        using (MailMessage msg = client.FetchMessage(info.UniqueUri))
                        {
                            Console.WriteLine($"Subject: {msg.Subject}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while listing or fetching messages: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
