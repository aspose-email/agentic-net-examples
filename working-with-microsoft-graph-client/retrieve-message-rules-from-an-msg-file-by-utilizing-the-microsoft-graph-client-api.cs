using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Initialize token provider with dummy credentials
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken");

                // Tenant identifier (dummy value)
                string tenantId = "tenantId";

                // Create Graph client
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Retrieve inbox rules
                    List<InboxRule> inboxRules = graphClient.ListRules();

                    // Output rule information
                    foreach (InboxRule rule in inboxRules)
                    {
                        Console.WriteLine("Rule ID: " + rule.RuleId);
                        Console.WriteLine("Display Name: " + rule.DisplayName);
                        Console.WriteLine("Enabled: " + rule.IsEnabled);
                        Console.WriteLine("Priority: " + rule.Priority);
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}