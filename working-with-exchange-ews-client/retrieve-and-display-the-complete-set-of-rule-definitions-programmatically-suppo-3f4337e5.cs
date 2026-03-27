using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Initialize the EWS client with placeholder credentials.
            string ewsUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, credentials))
            {
                // Retrieve all inbox rules for the default mailbox.
                InboxRule[] rules = client.GetInboxRules();

                // Display each rule's key properties.
                foreach (InboxRule rule in rules)
                {
                    Console.WriteLine("Rule ID      : " + rule.RuleId);
                    Console.WriteLine("Display Name : " + rule.DisplayName);
                    Console.WriteLine("Enabled      : " + rule.IsEnabled);
                    Console.WriteLine("Read Only    : " + rule.IsReadOnly);
                    Console.WriteLine("Priority     : " + rule.Priority);
                    Console.WriteLine("Has Conditions: " + (rule.Conditions != null));
                    Console.WriteLine("Has Actions   : " + (rule.Actions != null));
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}