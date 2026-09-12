using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

// Author: Sample code to delete a Gmail message using Aspose.Email

class Program
{
    static void Main()
    {
        try
        {
            // Replace with a valid OAuth access token and the email address of the account
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";

            // Create the Gmail client instance
            IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);

            // The ID of the message you want to delete
            string messageId = "MESSAGE_ID_TO_DELETE";

            // Delete the message permanently
            gmailClient.DeleteMessage(messageId);

            Console.WriteLine("Message deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
