using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Create and configure the IMAP client
            using (ImapClient imapClient = new ImapClient("imap.example.com", 993, "username", "password", SecurityOptions.SSLImplicit))
            {
                // Select the inbox folder
                imapClient.SelectFolder("INBOX");

                // Retrieve the list of messages in the folder
                ImapMessageInfoCollection messages = imapClient.ListMessages();

                // Iterate through each message and obtain its size
                foreach (ImapMessageInfo info in messages)
                {
                    long sizeInBytes = info.Size;
                    Console.WriteLine($"Message UID: {info.UniqueId}, Size: {sizeInBytes} bytes");

                    // Example of size‑based conditional handling
                    if (sizeInBytes > 1_048_576) // larger than 1 MB
                    {
                        Console.WriteLine("Large message detected – apply special processing.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
