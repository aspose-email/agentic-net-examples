using Aspose.Email.Clients.Exchange;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox connection settings
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client with safety guard
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Use the client within a using block to ensure disposal
            using (client)
            {
                // Retrieve list of message URIs from the Inbox folder
                string[] messageUris;
                try
                {
                    ExchangeMessageInfoCollection messagesInfo = client.ListMessages("Inbox");
                    messageUris = messagesInfo.Select(info => info.UniqueUri).ToArray();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                // Configuration for retry logic
                const int maxRetries = 3;
                const int delayMilliseconds = 2000;

                // Process each message with automatic retry on transient errors
                foreach (string uri in messageUris)
                {
                    int attempt = 0;
                    while (true)
                    {
                        try
                        {
                            MailMessage message = client.FetchMessage(uri);
                            Console.WriteLine($"Subject: {message.Subject}");
                            break; // Success, exit retry loop
                        }
                        catch (Exception ex) when (IsTransient(ex) && attempt < maxRetries)
                        {
                            attempt++;
                            Console.Error.WriteLine($"Transient error fetching message '{uri}'. Retry {attempt}/{maxRetries}: {ex.Message}");
                            Thread.Sleep(delayMilliseconds);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to fetch message '{uri}': {ex.Message}");
                            break; // Non‑transient error, move to next message
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Determines whether an exception is considered transient for retry purposes
    private static bool IsTransient(Exception ex)
    {
        if (ex is IOException)
            return true;
        if (ex is SocketException)
            return true;
        if (ex is WebException webEx)
        {
            return webEx.Status == WebExceptionStatus.ConnectFailure ||
                   webEx.Status == WebExceptionStatus.Timeout ||
                   webEx.Status == WebExceptionStatus.NameResolutionFailure;
        }
        return false;
    }
}
