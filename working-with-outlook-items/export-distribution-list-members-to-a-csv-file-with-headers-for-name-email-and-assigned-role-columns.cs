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
            // Placeholder credentials and distribution list identifier.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string distributionListId = "distribution-list-id";

            // Guard against placeholder values to avoid real network calls during CI.
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password" || distributionListId == "distribution-list-id")
            {
                Console.Error.WriteLine("Placeholder credentials or identifiers detected. Skipping execution.");
                return;
            }

            // Create the Exchange client.
            IEWSClient client;
            try
            {
                client = EWSClient.GetEWSClient(serviceUrl, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or connect Exchange client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Prepare the distribution list reference.
                ExchangeDistributionList distributionList = new ExchangeDistributionList
                {
                    Id = distributionListId
                };

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

                // Define CSV output path.
                string csvPath = "DistributionListMembers.csv";

                // Ensure the directory for the CSV file exists.
                try
                {
                    string directory = Path.GetDirectoryName(csvPath);
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

                // Write members to CSV.
                try
                {
                    using (StreamWriter writer = new StreamWriter(csvPath))
                    {
                        // Write CSV header.
                        writer.WriteLine("Name,Email,Role");

                        // Write each member.
                        foreach (MailAddress address in members)
                        {
                            string name = address.DisplayName ?? string.Empty;
                            string email = address.Address ?? string.Empty;
                            string role = string.Empty; // Role information not available in this context.
                            writer.WriteLine($"{EscapeCsv(name)},{EscapeCsv(email)},{EscapeCsv(role)}");
                        }
                    }

                    Console.WriteLine($"Distribution list members exported to '{csvPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write CSV file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper to escape CSV fields containing commas or quotes.
    private static string EscapeCsv(string field)
    {
        if (field.Contains("\""))
        {
            field = field.Replace("\"", "\"\"");
        }
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
        {
            field = $"\"{field}\"";
        }
        return field;
    }
}
