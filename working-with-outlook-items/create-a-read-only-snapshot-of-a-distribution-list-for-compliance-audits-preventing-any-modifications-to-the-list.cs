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
            // Placeholder credentials – skip execution in CI environments
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (exchangeUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder Exchange URI detected. Skipping execution.");
                return;
            }

            // Create and connect the EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(exchangeUri, username, password))
                {
                    // Retrieve all private distribution lists
                    ExchangeDistributionList[] distributionLists;
                    try
                    {
                        distributionLists = client.ListDistributionLists();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to list distribution lists: {ex.Message}");
                        return;
                    }

                    // Ensure output directory exists
                    string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "DistributionListSnapshots");
                    try
                    {
                        if (!Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                        return;
                    }

                    // Process each distribution list
                    foreach (ExchangeDistributionList dl in distributionLists)
                    {
                        // Fetch members of the current distribution list
                        MailAddressCollection members;
                        try
                        {
                            members = client.FetchDistributionList(dl);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to fetch members for list '{dl.DisplayName}': {ex.Message}");
                            continue;
                        }

                        // Prepare snapshot file path
                        string safeFileName = dl.DisplayName.Replace(Path.GetInvalidFileNameChars(), '_');
                        string snapshotPath = Path.Combine(outputDir, $"{safeFileName}_Snapshot.txt");

                        // Write members to snapshot file (read‑only snapshot)
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(snapshotPath, false))
                            {
                                writer.WriteLine($"Distribution List: {dl.DisplayName}");
                                writer.WriteLine("Members:");
                                foreach (MailAddress address in members)
                                {
                                    writer.WriteLine(address.Address);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to write snapshot for list '{dl.DisplayName}': {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or use EWS client: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

// Extension method to replace invalid filename characters
static class StringExtensions
{
    public static string Replace(this string str, char[] chars, char replacement)
    {
        foreach (char c in chars)
        {
            str = str.Replace(c, replacement);
        }
        return str;
    }
}
