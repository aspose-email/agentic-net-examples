using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.Collections.Generic;
using System.IO;

namespace AsposeEmailDistributionListReport
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – skip real network calls in CI environments
                string host = "exchange.example.com";
                string username = "username";
                string password = "password";

                if (host.Contains("example") || username.Contains("username") || password.Contains("password"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                    return;
                }

                // Connect to Exchange using EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(host, username, password))
                {
                    // Retrieve all private distribution lists
                    ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                    if (distributionLists == null || distributionLists.Length == 0)
                    {
                        Console.WriteLine("No distribution lists found.");
                        return;
                    }

                    // For demonstration, use the first distribution list
                    ExchangeDistributionList selectedList = distributionLists[0];

                    // Fetch members of the selected distribution list
                    MailAddressCollection members = client.FetchDistributionList(selectedList);

                    // Build report content
                    List<string> reportLines = new List<string>();
                    reportLines.Add("Distribution List Report");
                    reportLines.Add("========================");
                    reportLines.Add($"Display Name: {selectedList.DisplayName}");
                    reportLines.Add($"Id: {selectedList.Id}");
                    reportLines.Add($"ChangeKey: {selectedList.ChangeKey}");
                    reportLines.Add(string.Empty);
                    reportLines.Add("Members:");
                    foreach (MailAddress address in members)
                    {
                        reportLines.Add($"- {address.DisplayName} <{address.Address}>");
                    }

                    // Prepare output directory and file path
                    string outputDirectory = Path.Combine(Environment.CurrentDirectory, "Report");
                    try
                    {
                        if (!Directory.Exists(outputDirectory))
                        {
                            Directory.CreateDirectory(outputDirectory);
                        }
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                        return;
                    }

                    string outputPath = Path.Combine(outputDirectory, "DistributionListReport.pdf");

                    // Write report to file (PDF extension used for compatibility; content is plain text)
                    try
                    {
                        File.WriteAllLines(outputPath, reportLines);
                        Console.WriteLine($"Report saved to {outputPath}");
                    }
                    catch (Exception fileEx)
                    {
                        Console.Error.WriteLine($"Failed to write report file: {fileEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
