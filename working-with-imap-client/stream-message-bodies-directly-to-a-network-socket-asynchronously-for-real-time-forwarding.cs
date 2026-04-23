using Aspose.Email.Clients;
using System;
using System.IO;
using System.Net.Sockets;
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
            // Placeholder IMAP server credentials.
            string imapHost = "imap.example.com";
            int imapPort = 993;
            string imapUsername = "user@example.com";
            string imapPassword = "password";

            // Placeholder forwarding socket.
            string forwardHost = "forward.example.com";
            int forwardPort = 2525;

            // Guard against placeholder values to avoid real network calls during CI.
            if (imapHost.Contains("example.com") || forwardHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder host detected. Skipping network operations.");
                return;
            }

            // Create and use the IMAP client.
            using (ImapClient imapClient = new ImapClient(imapHost, imapPort, imapUsername, imapPassword, SecurityOptions.SSLImplicit))
            {
                try
                {
                    // Select the INBOX folder.
                    await imapClient.SelectFolderAsync("INBOX", null, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select folder: {ex.Message}");
                    return;
                }

                // Retrieve a limited set of message infos.
                ImapMessageInfoCollection messageInfos;
                try
                {
                    messageInfos = await imapClient.ListMessagesAsync(10, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                    return;
                }

                // Stream each message directly to the forwarding socket.
                foreach (ImapMessageInfo messageInfo in messageInfos)
                {
                    using (TcpClient tcpClient = new TcpClient())
                    {
                        try
                        {
                            await tcpClient.ConnectAsync(forwardHost, forwardPort);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to connect to forwarding host: {ex.Message}");
                            continue;
                        }

                        using (NetworkStream networkStream = tcpClient.GetStream())
                        {
                            try
                            {
                                // Stream the raw message content to the network socket.
                                await imapClient.SaveMessageAsync(messageInfo.UniqueId, networkStream);
                                await networkStream.FlushAsync();
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to forward message UID {messageInfo.UniqueId}: {ex.Message}");
                            }
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
}
