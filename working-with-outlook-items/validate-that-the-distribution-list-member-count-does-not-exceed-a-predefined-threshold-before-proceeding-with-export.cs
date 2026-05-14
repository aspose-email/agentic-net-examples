using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

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

            // Threshold for maximum allowed members
            int maxMemberCount = 100;

            // Output file for exported distribution lists
            string outputPath = "ExportedDistributionList.txt";

            // Detect placeholder credentials/host and skip external calls
            if (serviceUrl.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping export.");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // Retrieve all private distribution lists
                    ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                    foreach (ExchangeDistributionList dl in distributionLists)
                    {
                        // Fetch members of the current distribution list
                        MailAddressCollection members = client.FetchDistributionList(dl);

                        // Validate member count against the threshold
                        if (members.Count > maxMemberCount)
                        {
                            Console.WriteLine($"Distribution list '{dl.DisplayName}' has {members.Count} members, exceeding the limit of {maxMemberCount}. Skipping export.");
                            continue;
                        }

                        // Export the distribution list members to the output file
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(outputPath, true))
                            {
                                writer.WriteLine($"Distribution List: {dl.DisplayName}");
                                foreach (MailAddress address in members)
                                {
                                    writer.WriteLine($"{address.DisplayName} <{address.Address}>");
                                }
                                writer.WriteLine(); // Blank line between lists
                            }
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"Failed to write distribution list '{dl.DisplayName}' to file: {ioEx.Message}");
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"EWS client operation failed: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
