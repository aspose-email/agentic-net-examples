using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Gmail gmailClient
            IGmailClient gmailClient = GmailClient.GetInstance("clientId", "clientSecret", "refreshToken", "user@example.com");
            using (gmailClient)
            {
                try
                {
                    // Specify the message identifier to delete
                    string messageId = "MESSAGE_ID";
                    gmailClient.DeleteMessage(messageId);
                    Console.WriteLine($"Message {messageId} deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error deleting message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}