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
            // Define EWS connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Recipient of the calendar sharing invitation
                string externalRecipient = "external.user@otherdomain.com";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the sharing invitation as a MAPI message
                MapiMessage invitationMessage = client.CreateCalendarSharingInvitationMessage(externalRecipient);

                // Convert the MAPI message to a MailMessage for sending
                MailMessage mail = invitationMessage.ToMailMessage(new MailConversionOptions());

                // Send the invitation
                client.Send(mail);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
