using System;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the POP3 client with placeholder server and credentials.
            using (Pop3Client client = new Pop3Client("pop.example.com", "username", "password"))
            {
                // Example operation: retrieve and display the number of messages in the mailbox.
                int messageCount = client.GetMessageCount();
                Console.WriteLine($"Message count: {messageCount}");

                // The using block ensures that the client is properly disconnected
                // and all underlying network resources are released when the block exits.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
