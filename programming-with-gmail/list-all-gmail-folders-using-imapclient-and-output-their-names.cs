using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Base;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "your_email@gmail.com";

            // Guard against placeholder credentials to avoid real network calls during CI.
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken == "YOUR_ACCESS_TOKEN" ||
                string.IsNullOrWhiteSpace(defaultEmail) || defaultEmail == "your_email@gmail.com")
            {
                Console.Error.WriteLine("Gmail credentials are not provided. Skipping IMAP folder listing.");
                return;
            }

            // Create an IMAP client for Gmail using OAuth2 token.
            // Host: imap.gmail.com, Port: 993, UseOAuth = true, SSL implicit.
            using (ImapClient client = new ImapClient("imap.gmail.com", 993, defaultEmail, accessToken, true, SecurityOptions.SSLImplicit))
            {
                // List all folders in the mailbox.
                ImapFolderInfoCollection folders = client.ListFolders();

                Console.WriteLine("Gmail folders:");
                foreach (ImapFolderInfo folder in folders)
                {
                    Console.WriteLine("- " + folder.Name);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
