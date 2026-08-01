using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

// Author: Aspose.Email .NET sample
class Program
{
    static void Main()
    {
        // IMAP server connection settings
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

        ImapClient client = null;
        try
        {
            // Initialize the IMAP client with SSL implicit security
            client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit);

            // Select the INBOX folder
            client.SelectFolder("INBOX");

            // Retrieve messages from the selected folder
            ImapMessageInfoCollection messages = client.ListMessages();

            // Output basic information for each message
            foreach (var info in messages)
            {
                Console.WriteLine($"Subject: {info.Subject}, From: {info.From}");
            }
        }
        catch (Exception ex)
        {
            // Gracefully handle any errors
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
        finally
        {
            // Ensure the client is properly disposed
            if (client != null)
            {
                client.Dispose();
            }
        }
    }
}
