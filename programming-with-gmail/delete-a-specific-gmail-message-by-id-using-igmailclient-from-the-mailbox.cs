using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Guard against missing credentials.
            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(defaultEmail))
            {
                Console.Error.WriteLine("Gmail credentials are not provided. Skipping operation.");
                return;
            }

            // Create the Gmail client.
            IGmailClient gmailClient;
            try
            {
                gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Use the client and ensure it is disposed.
            using (gmailClient)
            {
                try
                {
                    // ID of the message to delete – replace with a real message ID.
                    string messageId = "MESSAGE_ID";

                    // Delete the message permanently.
                    gmailClient.DeleteMessage(messageId);
                    Console.WriteLine($"Message with ID '{messageId}' has been deleted.");
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
