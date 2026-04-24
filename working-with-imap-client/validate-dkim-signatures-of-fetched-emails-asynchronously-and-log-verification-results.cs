using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder IMAP server credentials
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Ensure the host is reachable by attempting a lightweight folder selection
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Select INBOX to validate credentials and connection
                    await client.SelectFolderAsync("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect or authenticate to IMAP server: {ex.Message}");
                    return;
                }

                // Retrieve message info collection from INBOX
                ImapMessageInfoCollection messageInfos;
                try
                {
                    messageInfos = await client.ListMessagesAsync("INBOX", false, null);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                if (messageInfos == null || messageInfos.Count == 0)
                {
                    Console.WriteLine("No messages found in INBOX.");
                    return;
                }

                // Collect sequence numbers for fetching
                List<int> sequenceNumbers = messageInfos.Select(info => info.SequenceNumber).ToList();

                // Fetch the full MailMessage objects asynchronously
                IList<MailMessage> mailMessages;
                try
                {
                    mailMessages = await client.FetchMessagesAsync(sequenceNumbers);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to fetch messages: {ex.Message}");
                    return;
                }

                // Verify DKIM signature for each message
                foreach (MailMessage mailMessage in mailMessages)
                {
                    try
                    {
                        // CheckSignature returns an array of X509Certificate2 if a signature is present and valid
                        var certificates = mailMessage.CheckSignature();
                        if (certificates != null && certificates.Length > 0)
                        {
                            Console.WriteLine($"Message '{mailMessage.Subject}' has a valid DKIM signature.");
                        }
                        else
                        {
                            Console.WriteLine($"Message '{mailMessage.Subject}' does not contain a valid DKIM signature.");
                        }
                    }
                    catch (Exception sigEx)
                    {
                        Console.Error.WriteLine($"Error verifying DKIM for message '{mailMessage.Subject}': {sigEx.Message}");
                    }
                    finally
                    {
                        // Dispose each MailMessage after processing
                        mailMessage.Dispose();
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
