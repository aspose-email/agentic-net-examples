using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing with placeholder credentials
            if (serviceUrl.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external service call.");
                return;
            }

            // Output JSON file path
            string outputPath = "distributionListMembers.json";

            // Ensure output directory exists
            try
            {
                string? directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Create and use the EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Prepare a distribution list object (replace with real Id if available)
                    ExchangeDistributionList distributionList = new ExchangeDistributionList();

                    // Fetch members of the distribution list
                    MailAddressCollection members = client.FetchDistributionList(distributionList);

                    // Transform members into a serializable structure
                    List<MemberInfo> memberInfos = new List<MemberInfo>();
                    foreach (MailAddress address in members)
                    {
                        memberInfos.Add(new MemberInfo
                        {
                            Email = address.Address,
                            DisplayName = address.DisplayName
                        });
                    }

                    // Serialize to JSON
                    string json = JsonSerializer.Serialize(memberInfos, new JsonSerializerOptions { WriteIndented = true });

                    // Write JSON to file
                    try
                    {
                        File.WriteAllText(outputPath, json);
                        Console.WriteLine($"Distribution list members exported to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write JSON file: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper class for JSON serialization
    private class MemberInfo
    {
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
