using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

namespace ExportVoteResults
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder Exchange connection details
                string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard against placeholder credentials to avoid real network calls
                if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping network operations.");
                    // Proceed with dummy data export
                }
                else
                {
                    // Create and use ExchangeClient safely
                    using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                    {
                        // Example: fetch messages from a folder (omitted for brevity)
                        // ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");
                    }
                }

                // Prepare dummy vote results data
                List<VoteResult> results = new List<VoteResult>
                {
                    new VoteResult { Option = "Option A", Votes = 120, Reactions = 45 },
                    new VoteResult { Option = "Option B", Votes = 85,  Reactions = 30 },
                    new VoteResult { Option = "Option C", Votes = 60,  Reactions = 20 }
                };

                // Define output CSV file path (Excel can open CSV files)
                string csvPath = "VoteResults.csv";

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(csvPath));
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Export data to CSV
                using (var writer = new StreamWriter(csvPath))
                {
                    // Write header row
                    writer.WriteLine("Option,Votes,Reactions");

                    // Write data rows
                    foreach (var result in results)
                    {
                        writer.WriteLine($"{EscapeCsv(result.Option)},{result.Votes},{result.Reactions}");
                    }
                }

                Console.WriteLine($"Vote results exported successfully to '{csvPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        // Simple DTO for vote results
        private class VoteResult
        {
            public string Option { get; set; }
            public int Votes { get; set; }
            public int Reactions { get; set; }
        }

        // Helper to escape CSV fields containing commas or quotes
        private static string EscapeCsv(string field)
        {
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                field = field.Replace("\"", "\"\"");
                return $"\"{field}\"";
            }
            return field;
        }
    }
}
