using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Output CSV file path
            string outputPath = "DistributionListMembers.csv";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve all private distribution lists
                ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                // Open CSV writer
                using (StreamWriter writer = new StreamWriter(outputPath, false, Encoding.UTF8))
                {
                    // Write CSV header
                    writer.WriteLine("DistributionList,DisplayName,EmailAddress");

                    // Iterate through each distribution list
                    foreach (ExchangeDistributionList dl in distributionLists)
                    {
                        // Fetch members of the current distribution list
                        MailAddressCollection members = client.FetchDistributionList(dl);

                        // Write each member to the CSV
                        foreach (MailAddress member in members)
                        {
                            writer.WriteLine($"{dl.DisplayName},{member.DisplayName},{member.Address}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
