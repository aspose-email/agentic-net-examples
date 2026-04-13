using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server URI and user credentials
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // URI of the meeting request message to inspect
                string messageUri = "https://mail.example.com/EWS/MessageId";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || messageUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Fetch the meeting request as a MailMessage
                MailMessage meetingRequest = client.FetchMessage(messageUri);

                // Retrieve the 'Location' MIME header (null if not present)
                string location = meetingRequest.Headers["Location"];

                Console.WriteLine("Location header: " + (location ?? "Not found"));
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
