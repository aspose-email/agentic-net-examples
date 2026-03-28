using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string msgFilePath = "Invitation.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(msgFilePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {outputDirectory}. {dirEx.Message}");
                    return;
                }
            }

            // Recipient email address for the sharing invitation
            string recipientEmail = "user@example.com";

            // Create and connect the EWS client
            IEWSClient ewsClient = null;
            try
            {
                ewsClient = EWSClient.GetEWSClient(
                    "https://exchange.example.com/EWS/Exchange.asmx",
                    "username",
                    "password");
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Error: Unable to connect to Exchange – {clientEx.Message}");
                return;
            }

            // Use the client within a using block to ensure disposal
            using (ewsClient)
            {
                // Create the calendar sharing invitation message
                MapiMessage invitationMessage = null;
                try
                {
                    invitationMessage = ewsClient.CreateCalendarSharingInvitationMessage(recipientEmail);
                }
                catch (Exception invEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create invitation – {invEx.Message}");
                    return;
                }

                // Save the invitation as an MSG file
                try
                {
                    invitationMessage.Save(msgFilePath);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Error: Unable to save MSG file – {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
