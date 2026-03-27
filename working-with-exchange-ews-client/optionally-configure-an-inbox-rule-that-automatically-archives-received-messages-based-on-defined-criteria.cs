using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailInboxRuleExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define EWS connection parameters
                string ewsUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create the EWS client inside a using block to ensure disposal
                using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, username, password))
                {
                    // Define criteria for the rule (messages containing these keywords)
                    string[] keywords = new string[] { "Invoice", "Report" };

                    // Destination folder for archiving (must exist on the mailbox)
                    string archiveFolder = "Archive";

                    // Create an inbox rule that moves matching messages to the archive folder
                    InboxRule rule = InboxRule.CreateRuleMoveContaining(keywords, archiveFolder);

                    // Apply the rule to the mailbox
                    client.CreateInboxRule(rule);

                    Console.WriteLine("Inbox rule created successfully to archive matching messages.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
