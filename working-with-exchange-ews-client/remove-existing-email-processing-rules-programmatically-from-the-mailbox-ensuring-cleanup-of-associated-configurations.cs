using System;
using System.Net;
using Aspose.Email;
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
                // Define mailbox connection parameters (replace with actual values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create the EWS client inside a using block to ensure proper disposal
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    try
                    {
                        // Retrieve all inbox rules for the default mailbox
                        InboxRule[] inboxRules = client.GetInboxRules();

                        // Delete each rule by its identifier
                        foreach (InboxRule rule in inboxRules)
                        {
                            if (!string.IsNullOrEmpty(rule.RuleId))
                            {
                                client.DeleteInboxRule(rule.RuleId);
                            }
                        }

                        Console.WriteLine("All inbox rules have been removed successfully.");
                    }
                    catch (Exception ex)
                    {
                        // Handle errors that occur while processing rules
                        Console.Error.WriteLine("Error processing inbox rules: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors that occur during client creation or connection
                Console.Error.WriteLine("Failed to connect to Exchange server: " + ex.Message);
            }
        }
    }
}