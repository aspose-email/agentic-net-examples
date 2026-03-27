using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize POP3 client with placeholder credentials
            using (Pop3Client client = new Pop3Client("pop.example.com", 110, "username", "password", SecurityOptions.Auto))
            {
                try
                {
                    // Perform a simple operation to ensure the session is established
                    int messageCount = client.GetMessageCount();
                    Console.WriteLine($"Message count: {messageCount}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation failed: {ex.Message}");
                    return;
                }
                // The using block will dispose the client, effectively disconnecting the session
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
