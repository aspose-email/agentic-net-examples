using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize and connect the IMAP client
            using (ImapClient client = new ImapClient())
            {
                try
                {
                    client.Host = "imap.example.com";
                    client.Port = 993;
                    client.SecurityOptions = SecurityOptions.SSLImplicit;
                    client.Username = "username";
                    client.Password = "password";
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to connect IMAP client: {ex.Message}");
                    return;
                }

                try
                {
                    // Select the INBOX folder
                    client.SelectFolder("INBOX");

                    // Retrieve message information objects
                    var messages = client.ListMessages();

                    foreach (var msgInfo in messages)
                    {
                        // Get the size of the current email message in bytes
                        long sizeInBytes = msgInfo.Size;
                        Console.WriteLine($"Message ID: {msgInfo.UniqueId}, Size: {sizeInBytes} bytes");

                        // Conditional handling based on size (example: flag large messages)
                        if (sizeInBytes > 1_048_576) // larger than 1 MB
                        {
                            Console.WriteLine("Large message detected.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
