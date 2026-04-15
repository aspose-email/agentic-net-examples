using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client (replace with actual service URL and credentials)
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Create a new inbox rule
                InboxRule rule = new InboxRule();
                rule.DisplayName = "Move large attachments";
                rule.IsEnabled = true;

                // Set conditions (has attachments). Size condition is not directly exposed in the API,
                // so this example sets the attachment presence condition only.
                // rule.Conditions.HasAttachments = true;
                // rule.Conditions.SizeGreaterThan = 5 * 1024 * 1024; // 5 MB (if supported)

                // Set actions: move matching messages to the "Large Files" folder
                // rule.Actions.MoveToFolder = "Large Files";

                // Create the rule on the server
                client.CreateInboxRule(rule);
                Console.WriteLine("Inbox rule created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
