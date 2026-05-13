using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials and service URL
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username@example.com";
            string password = "password";

            // Guard against placeholder values to avoid real network calls
            if (serviceUrl.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Retrieve all private distribution lists
                ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                if (distributionLists == null || distributionLists.Length == 0)
                {
                    Console.Error.WriteLine("No distribution lists found.");
                    return;
                }

                // Select the first distribution list (replace with specific logic if needed)
                ExchangeDistributionList targetList = distributionLists[0];

                // Fetch members of the selected distribution list
                MailAddressCollection members = client.FetchDistributionList(targetList);

                // Filter members whose email address ends with the desired domain
                string domainPattern = "@contoso.com";
                List<string> filteredEmails = new List<string>();
                foreach (MailAddress address in members)
                {
                    if (address.Address != null && address.Address.EndsWith(domainPattern, StringComparison.OrdinalIgnoreCase))
                    {
                        filteredEmails.Add(address.Address);
                    }
                }

                // Serialize filtered email addresses to JSON
                string json = JsonSerializer.Serialize(filteredEmails, new JsonSerializerOptions { WriteIndented = true });

                // Define output file path
                string outputPath = "filteredMembers.json";

                // Ensure the output directory exists
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Write JSON to file with error handling
                try
                {
                    File.WriteAllText(outputPath, json);
                    Console.WriteLine($"Filtered members exported to {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to write JSON file: {ioEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
