using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Imap.Models;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder IMAP server credentials
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when placeholders are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Create and configure the IMAP client
            using (ImapClient imapClient = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                // Ensure the client connection is safe
                try
                {
                    // Select the Inbox folder (no explicit Connect call required)
                    Stopwatch swSelect = Stopwatch.StartNew();
                    await imapClient.SelectFolderAsync("INBOX");
                    swSelect.Stop();
                    Console.WriteLine($"SelectFolderAsync duration: {swSelect.ElapsedMilliseconds} ms");

                    // List messages asynchronously
                    Stopwatch swList = Stopwatch.StartNew();
                    ImapMessageInfoCollection messages = await imapClient.ListMessagesAsync();
                    swList.Stop();
                    Console.WriteLine($"ListMessagesAsync duration: {swList.ElapsedMilliseconds} ms");
                    Console.WriteLine($"Total messages retrieved: {messages.Count}");

                    if (messages.Count > 0)
                    {
                        // Fetch the first message by UniqueId
                        string firstUid = messages[0].UniqueId;
                        Stopwatch swFetch = Stopwatch.StartNew();
                        MailMessage fetchedMessage = await imapClient.FetchMessageAsync(firstUid);
                        swFetch.Stop();
                        Console.WriteLine($"FetchMessageAsync (UID={firstUid}) duration: {swFetch.ElapsedMilliseconds} ms");
                        Console.WriteLine($"Subject: {fetchedMessage.Subject}");

                        // Delete the fetched message (demonstration only)
                        Stopwatch swDelete = Stopwatch.StartNew();
                        await imapClient.DeleteMessageAsync(firstUid);
                        swDelete.Stop();
                        Console.WriteLine($"DeleteMessageAsync (UID={firstUid}) duration: {swDelete.ElapsedMilliseconds} ms");
                    }

                    // Validate credentials asynchronously
                    Stopwatch swValidate = Stopwatch.StartNew();
                    await imapClient.ValidateCredentialsAsync();
                    swValidate.Stop();
                    Console.WriteLine($"ValidateCredentialsAsync duration: {swValidate.ElapsedMilliseconds} ms");
                }
                catch (ImapException imapEx)
                {
                    Console.Error.WriteLine($"IMAP error: {imapEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
