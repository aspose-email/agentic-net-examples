using Aspose.Email.Storage.Pst;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

public class Program
{
    // Author: Aspose.Email example – asynchronous IMAP connection and folder listing
    public static async Task Main(string[] args)
    {
        try
        {
            // IMAP server configuration (replace with real credentials)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the IMAP client and configure security
            using (ImapClient imapClient = new ImapClient())
            {
                imapClient.Host = host;
                imapClient.Port = port;
                imapClient.Username = username;
                imapClient.Password = password;
                imapClient.SecurityOptions = SecurityOptions.SSLImplicit;

                // List folders asynchronously to avoid blocking the calling thread
                try
                {
                    IList<ImapFolderInfo> folders = await Task.Run(() => imapClient.ListFolders());
                    Console.WriteLine("Folders on the IMAP server:");
                    foreach (ImapFolderInfo folder in folders)
                    {
                        Console.WriteLine($"- {folder.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error listing folders: {ex.Message}");
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
