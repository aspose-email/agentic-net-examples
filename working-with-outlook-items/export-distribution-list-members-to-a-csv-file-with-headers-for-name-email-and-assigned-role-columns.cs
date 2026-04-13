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
            // Placeholder credentials and distribution list identifier
            string host = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string distributionListId = "placeholder-distribution-list-id";
            string csvOutputPath = "DistributionListMembers.csv";

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com") || username.Contains("example.com") || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(distributionListId))
            {
                Console.Error.WriteLine("Placeholder credentials or distribution list ID detected. Skipping execution.");
                return;
            }

            // Ensure the output directory exists
            try
            {
                string outputDirectory = Path.GetDirectoryName(csvOutputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Create and use the EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(host, username, password))
                {
                    // Prepare the distribution list object with the known Id
                    ExchangeDistributionList distributionList = new ExchangeDistributionList();
                    distributionList.Id = distributionListId;

                    // Fetch members of the private distribution list
                    MailAddressCollection members = client.FetchDistributionList(distributionList);

                    // Write members to CSV
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(csvOutputPath, false))
                        {
                            // Write CSV header
                            writer.WriteLine("Name,Email,Role");

                            foreach (MailAddress member in members)
                            {
                                // Role information is not provided by MailAddress; leave empty
                                string line = $"{EscapeCsv(member.DisplayName)},{EscapeCsv(member.Address)},";
                                writer.WriteLine(line);
                            }
                        }

                        Console.WriteLine($"Distribution list members exported to '{csvOutputPath}'.");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Error writing CSV file: {ioEx.Message}");
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Error connecting to Exchange server: {clientEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper method to escape CSV fields containing commas or quotes
    private static string EscapeCsv(string field)
    {
        if (field == null)
            return string.Empty;

        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            string escaped = field.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }
        return field;
    }
}
