using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize POP3 client with host, username and password
            using (Pop3Client client = new Pop3Client("pop3.example.com", "username", "password"))
            {
                try
                {
                    // Retrieve the list of messages from the server
                    Pop3MessageInfoCollection messages = client.ListMessages();

                    // Iterate through the messages and display their unique identifiers
                    foreach (Pop3MessageInfo info in messages)
                    {
                        Console.WriteLine($"Message UID: {info.UniqueId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while listing messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
