using System;
using System.Net;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Create a MAPI message that will hold the OOF reply
                MapiMessage oofMessage = new MapiMessage("sender@example.com", "recipient@example.com", "Out of Office", "I am currently out of the office and will reply upon my return.");

                // Add the custom OOF property (PR_EMS_AB_OOF_REPLY_TO_ORIGINATOR)
                // Use PT_UNICODE and encode the reply text as Unicode bytes
                oofMessage.AddCustomProperty(
                    MapiPropertyType.PT_UNICODE,
                    Encoding.Unicode.GetBytes("I am currently out of the office and will reply upon my return."),
                    "PR_EMS_AB_OOF_REPLY_TO_ORIGINATOR");

                // Append the message as a draft to the Drafts folder and mark it as sent (OOF settings are applied when sent)
                // The Drafts folder URI can be obtained from the mailbox info
                string draftsFolderUri = client.MailboxInfo.DraftsUri;
                client.AppendMessage(draftsFolderUri, oofMessage, true);

                Console.WriteLine("Custom out-of-office automatic reply has been set.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
