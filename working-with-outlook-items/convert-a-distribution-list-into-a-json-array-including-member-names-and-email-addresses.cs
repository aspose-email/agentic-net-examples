using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace DistributionListToJson
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder connection settings
                string host = "exchange.example.com";
                string username = "user@example.com";
                string password = "password";

                // Skip external call if placeholders are detected
                if (host.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                    return;
                }

                // Create and connect the Exchange client
                using (IEWSClient client = EWSClient.GetEWSClient(host, username, password))
                {
                    try
                    {
                        // List private distribution lists
                        ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                        if (distributionLists == null || distributionLists.Length == 0)
                        {
                            Console.Error.WriteLine("No distribution lists found.");
                            return;
                        }

                        // Use the first distribution list (adjust as needed)
                        ExchangeDistributionList targetList = distributionLists[0];

                        // Fetch members of the selected distribution list
                        MailAddressCollection members = client.FetchDistributionList(targetList);

                        // Prepare a list of simple objects for JSON serialization
                        List<MemberInfo> memberInfoList = new List<MemberInfo>();

                        foreach (MailAddress address in members)
                        {
                            MemberInfo info = new MemberInfo();
                            info.Name = address.DisplayName;
                            info.Email = address.Address;
                            memberInfoList.Add(info);
                        }

                        // Serialize to JSON
                        string json = JsonSerializer.Serialize(memberInfoList, new JsonSerializerOptions { WriteIndented = true });

                        // Define output path
                        string outputPath = "distributionList.json";

                        // Ensure directory exists
                        string directory = Path.GetDirectoryName(outputPath);
                        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        // Write JSON to file with guarded IO
                        try
                        {
                            File.WriteAllText(outputPath, json);
                            Console.WriteLine($"Distribution list exported to {outputPath}");
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"Failed to write JSON file: {ioEx.Message}");
                        }
                    }
                    catch (Exception clientEx)
                    {
                        Console.Error.WriteLine($"Exchange operation failed: {clientEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        // Simple DTO for JSON output
        private class MemberInfo
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }
    }
}
