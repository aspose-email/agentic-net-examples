using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Create the meeting request message
                MailMessage message = new MailMessage();
                message.From = new MailAddress("organizer@example.com");
                message.To.Add("attendee1@example.com");
                message.To.Add("attendee2@example.com");
                message.Subject = "Team Meeting – Please select a time";
                message.Body = "Dear team,\n\nPlease vote for the most convenient meeting time.\n\nThank you.";

                // Define voting options (poll)
                FollowUpOptions options = new FollowUpOptions();
                options.VotingButtons = "9:00 AM;10:00 AM;11:00 AM";

                // Send the meeting request with voting options
                try
                {
                    client.Send(message, options);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send meeting request: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
