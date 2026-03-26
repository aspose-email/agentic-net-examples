using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Exchange server connection parameters (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create the EWS client inside a using block to ensure disposal
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Retrieve all inbox rules for the default mailbox
                        InboxRule[] inboxRules = client.GetInboxRules();

                        Console.WriteLine("Current Inbox Rules:");
                        foreach (InboxRule rule in inboxRules)
                        {
                            Console.WriteLine($"- {rule.DisplayName} (Enabled: {rule.IsEnabled})");
                        }

                        // Example: Update the first rule (if any) to disable it
                        if (inboxRules.Length > 0)
                        {
                            InboxRule ruleToUpdate = inboxRules[0];
                            ruleToUpdate.IsEnabled = false;
                            client.UpdateInboxRule(ruleToUpdate);
                            Console.WriteLine($"Rule \"{ruleToUpdate.DisplayName}\" has been disabled.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}