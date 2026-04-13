using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls.
            if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Output file for the read‑only snapshot.
            string outputPath = Path.Combine(Environment.CurrentDirectory, "DistributionListSnapshot.csv");

            // Ensure the output directory exists.
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

            // Connect to Exchange and fetch distribution list members.
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Retrieve all private distribution lists.
                    ExchangeDistributionList[] lists = client.ListDistributionLists();
                    if (lists == null || lists.Length == 0)
                    {
                        Console.Error.WriteLine("No distribution lists found.");
                        return;
                    }

                    // For demonstration, use the first distribution list.
                    ExchangeDistributionList targetList = lists[0];

                    // Fetch members of the selected distribution list.
                    MailAddressCollection members = client.FetchDistributionList(targetList);
                    if (members == null || members.Count == 0)
                    {
                        Console.Error.WriteLine("The distribution list contains no members.");
                        return;
                    }

                    // Write members to CSV in a read‑only manner.
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(outputPath, false))
                        {
                            writer.WriteLine("DisplayName,EmailAddress");
                            foreach (var address in members)
                            {
                                // MailAddress in Aspose.Email provides DisplayName and Address.
                                string displayName = address.DisplayName?.Replace(",", " ") ?? string.Empty;
                                string email = address.Address ?? string.Empty;
                                writer.WriteLine($"{displayName},{email}");
                            }
                        }

                        // Make the snapshot file read‑only.
                        File.SetAttributes(outputPath, FileAttributes.ReadOnly);
                        Console.WriteLine($"Read‑only snapshot created at: {outputPath}");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Failed to write snapshot file: {ioEx.Message}");
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Exchange client error: {clientEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
