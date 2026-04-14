using System;
using System.IO;
using System.Net;
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
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            ICredentials credentials = new NetworkCredential("username", "password");

            // Create and connect EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // List all private distribution lists
                    ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                    // Find the distribution list to delete (example: by display name)
                    string targetDisplayName = "Sample Distribution List";
                    ExchangeDistributionList targetList = null;
                    foreach (ExchangeDistributionList list in distributionLists)
                    {
                        if (list.DisplayName == targetDisplayName)
                        {
                            targetList = list;
                            break;
                        }
                    }

                    if (targetList == null)
                    {
                        Console.Error.WriteLine($"Distribution list \"{targetDisplayName}\" not found.");
                        return;
                    }

                    // Fetch members of the distribution list
                    MailAddressCollection members = client.FetchDistributionList(targetList);

                    // Prepare data for JSON serialization
                    List<Dictionary<string, string>> memberData = new List<Dictionary<string, string>>();
                    foreach (MailAddress address in members)
                    {
                        var entry = new Dictionary<string, string>
                        {
                            { "DisplayName", address.DisplayName ?? string.Empty },
                            { "Email", address.Address ?? string.Empty }
                        };
                        memberData.Add(entry);
                    }

                    // Define JSON file path
                    string jsonFilePath = "DistributionListMembers.json";


                    // Skip external calls when placeholder credentials are used
                    if (mailboxUri.Contains("example.com"))
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    // Ensure directory exists
                    try
                    {
                        string directory = Path.GetDirectoryName(jsonFilePath);
                        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        // Serialize to JSON and write to file
                        string jsonContent = JsonSerializer.Serialize(memberData, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(jsonFilePath, jsonContent);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write JSON file: {ex.Message}");
                        return;
                    }

                    // Delete the distribution list permanently
                    try
                    {
                        client.DeleteDistributionList(targetList, true);
                        Console.WriteLine($"Distribution list \"{targetDisplayName}\" deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete distribution list: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to connect to EWS: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
