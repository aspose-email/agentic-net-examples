using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define mailbox connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            ICredentials credentials = new NetworkCredential("username", "password");

            // Create the EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Build a query to find messages older than 6 months
                DateTime cutoffDate = DateTime.Now.AddMonths(-6);
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.InternalDate.Before(cutoffDate);
                MailQuery query = builder.GetQuery();

                // Retrieve messages from the Inbox that match the query
                var messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                // Prepare a temporary folder to store the exported messages
                string tempFolder = Path.Combine(Path.GetTempPath(), "ArchiveTemp");
                if (!Directory.Exists(tempFolder))
                {
                    Directory.CreateDirectory(tempFolder);
                }

                // Export each message to an .eml file
                foreach (ExchangeMessageInfo info in messages)
                {
                    try
                    {
                        // Generate a unique file name for each message
                        string emlPath = Path.Combine(tempFolder, Guid.NewGuid().ToString() + ".eml");

                        // Fetch the full message and save it
                        using (MailMessage message = client.FetchMessage(info.UniqueUri))
                        {
                            message.Save(emlPath, SaveOptions.DefaultEml);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to export message {info.UniqueUri}: {ex.Message}");
                    }
                }

                // Create a ZIP archive from the exported messages
                string zipPath = Path.Combine(Environment.CurrentDirectory, "ArchivedEmails.zip");
                try
                {
                    if (File.Exists(zipPath))
                    {
                        File.Delete(zipPath);
                    }
                    ZipFile.CreateFromDirectory(tempFolder, zipPath);
                    Console.WriteLine($"Archived emails saved to: {zipPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create ZIP archive: {ex.Message}");
                }

                // Clean up the temporary folder
                try
                {
                    if (Directory.Exists(tempFolder))
                    {
                        Directory.Delete(tempFolder, true);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to clean up temporary folder: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}