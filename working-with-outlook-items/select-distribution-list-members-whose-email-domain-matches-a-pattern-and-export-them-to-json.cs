using System;
using System.Collections.Generic;
using System.IO;
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
            // Placeholder credentials – skip real network call if they are not replaced.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string userName = "user@example.com";
            string password = "password";

            if (serviceUrl.Contains("example"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping EWS operation.");
                return;
            }

            // Create EWS client inside a try/catch to handle connection issues.
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, userName, password))
                {
                    // Prepare a distribution list identifier (replace with a real Id).
                    ExchangeDistributionList distributionList = new ExchangeDistributionList();
                    distributionList.Id = "distributionlist-id";

                    // Fetch members of the private distribution list.
                    MailAddressCollection members;
                    try
                    {
                        members = client.FetchDistributionList(distributionList);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch distribution list members: {ex.Message}");
                        return;
                    }

                    // Filter members whose email address ends with the desired domain.
                    List<Dictionary<string, string>> filteredMembers = new List<Dictionary<string, string>>();
                    foreach (MailAddress address in members)
                    {
                        if (address.Address != null && address.Address.EndsWith("@example.com", StringComparison.OrdinalIgnoreCase))
                        {
                            Dictionary<string, string> entry = new Dictionary<string, string>();
                            entry["DisplayName"] = address.DisplayName ?? string.Empty;
                            entry["Email"] = address.Address;
                            filteredMembers.Add(entry);
                        }
                    }

                    // Serialize the filtered list to JSON.
                    string json = JsonSerializer.Serialize(filteredMembers, new JsonSerializerOptions { WriteIndented = true });

                    // Define output path and ensure the directory exists.
                    string outputPath = "filteredMembers.json";
                    string outputDirectory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        try
                        {
                            Directory.CreateDirectory(outputDirectory);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                            return;
                        }
                    }

                    // Write JSON to file with error handling.
                    try
                    {
                        File.WriteAllText(outputPath, json);
                        Console.WriteLine($"Filtered members exported to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write JSON file: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS client initialization error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
