using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;
using System.Net;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            // Skip external call when placeholders are detected.
            if (clientId.Contains("your-") || clientSecret.Contains("your-") || refreshToken.Contains("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client call.");
                return;
            }

            // Create Gmail client.
            IGmailClient gmailClient = null;
            try
            {
                // Added null for optional proxy parameter to match overload.
                gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, null);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Fetch all contacts.
            Contact[] contacts = null;
            try
            {
                contacts = gmailClient.GetAllContacts();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to fetch contacts: {ex.Message}");
                return;
            }
            finally
            {
                if (gmailClient is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }

            // Compute statistics.
            int totalCount = contacts != null ? contacts.Length : 0;
            var companyCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            double averageAge = 0.0; // Age information is not available in Contact; default to 0.

            if (contacts != null)
            {
                foreach (Contact contact in contacts)
                {
                    string company = contact.CompanyName ?? "Unknown";
                    if (companyCounts.ContainsKey(company))
                        companyCounts[company]++;
                    else
                        companyCounts[company] = 1;
                }
            }

            // Prepare summary object.
            var summary = new
            {
                TotalCount = totalCount,
                Companies = companyCounts,
                AverageAge = averageAge
            };

            // Serialize to JSON.
            string json = JsonSerializer.Serialize(summary, new JsonSerializerOptions { WriteIndented = true });

            // Define output path.
            string outputPath = "contact_summary.json";

            // Ensure directory exists.
            try
            {
                string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to ensure output directory: {ex.Message}");
                return;
            }

            // Write JSON to file.
            try
            {
                File.WriteAllText(outputPath, json);
                Console.WriteLine($"Contact summary written to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write summary file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
