using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        // Mailbox connection parameters (replace with real credentials)
        string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
        string username = "user@example.com";
        string password = "password";

        // Guard: skip network operations when placeholder credentials are detected
        bool isPlaceholder = username.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                             password.Equals("password", StringComparison.OrdinalIgnoreCase);

        if (isPlaceholder)
        {
            Console.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
            return;
        }

        try
        {
            // Create EWS client and ensure proper disposal
            using (IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Define the IDs of the inbox rules to delete.
                // Replace these placeholder IDs with the actual rule identifiers obtained from the mailbox.
                string[] ruleIds = new string[] { "rule-id-1", "rule-id-2" };

                foreach (string ruleId in ruleIds)
                {
                    try
                    {
                        // Delete the rule by its identifier
                        ewsClient.DeleteInboxRule(ruleId);
                        Console.WriteLine($"Deleted inbox rule with ID: {ruleId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete rule '{ruleId}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
