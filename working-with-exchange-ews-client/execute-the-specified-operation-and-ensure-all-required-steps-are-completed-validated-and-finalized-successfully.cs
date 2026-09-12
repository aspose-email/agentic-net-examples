using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailEwsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input MSG file path
                string msgFilePath = "sample.msg";

                // Verify the input file exists
                if (!File.Exists(msgFilePath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                    return;
                }

                // Load the MSG file as a MapiMessage
                MapiMessage mapiMessage = MapiMessage.Load(msgFilePath);

                // Convert MapiMessage to MailMessage for EWS operations
                MailConversionOptions conversionOptions = new MailConversionOptions();
                MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions);

                // EWS service connection parameters (replace with real values if needed)
                string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create and use the EWS client
                using (IEWSClient ewsClient = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = ewsClient.GetMailboxInfo();

                    // Append the message to the Drafts folder
                    ewsClient.AppendMessage(mailboxInfo.DraftsUri, mailMessage);

                    Console.WriteLine("Message appended to Drafts successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
