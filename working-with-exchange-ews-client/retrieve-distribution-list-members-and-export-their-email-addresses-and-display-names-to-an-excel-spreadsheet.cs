using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace DistributionListExport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // EWS service configuration (replace with actual values)
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Output CSV file path
                string outputPath = "DistributionListMembers.csv";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Create and use the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // List all private distribution lists
                    ExchangeDistributionList[] lists = client.ListDistributionLists();

                    if (lists == null || lists.Length == 0)
                    {
                        Console.Error.WriteLine("No distribution lists found.");
                        return;
                    }

                    // For demonstration, use the first distribution list
                    ExchangeDistributionList targetList = lists[0];

                    // Fetch members of the selected distribution list
                    MailAddressCollection members = client.FetchDistributionList(targetList);

                    // Write members to CSV (Excel can open CSV files)
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(outputPath, false, Encoding.UTF8))
                        {
                            // Header
                            writer.WriteLine("Display Name,Email Address");

                            foreach (MailAddress address in members)
                            {
                                // Some MailAddress objects may not have a display name; handle nulls
                                string displayName = address.DisplayName ?? string.Empty;
                                string email = address.Address ?? string.Empty;

                                // Escape commas in display name if needed
                                if (displayName.Contains(","))
                                {
                                    displayName = $"\"{displayName}\"";
                                }

                                writer.WriteLine($"{displayName},{email}");
                            }
                        }

                        Console.WriteLine($"Distribution list members exported to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error writing to file: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
