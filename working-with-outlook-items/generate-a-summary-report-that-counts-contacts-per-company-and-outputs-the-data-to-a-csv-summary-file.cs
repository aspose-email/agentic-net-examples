using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";

            // Guard against executing with placeholder credentials.
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder Gmail credentials detected. Skipping contact retrieval.");
                return;
            }

            // Create Gmail client.
            IGmailClient client;
            try
            {
                // The fourth parameter is an optional IWebProxy; passing null uses the default.
                client = GmailClient.GetInstance(clientId, clientSecret, refreshToken, null);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Retrieve all contacts.
            Contact[] contacts;
            try
            {
                contacts = client.GetAllContacts();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to retrieve contacts: {ex.Message}");
                return;
            }

            // Count contacts per company.
            var companyCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (Contact contact in contacts)
            {
                string company = contact.CompanyName;
                if (string.IsNullOrWhiteSpace(company))
                    continue;

                if (companyCounts.ContainsKey(company))
                    companyCounts[company]++;
                else
                    companyCounts[company] = 1;
            }

            // Prepare CSV output.
            string outputPath = "ContactSummary.csv";
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory '{directory}': {ex.Message}");
                    return;
                }
            }

            // Write CSV file.
            try
            {
                using (var writer = new StreamWriter(outputPath, false))
                {
                    writer.WriteLine("Company,Count");
                    foreach (var kvp in companyCounts.OrderBy(k => k.Key))
                    {
                        // Escape commas in company names if necessary.
                        string escapedCompany = kvp.Key.Contains(",") ? $"\"{kvp.Key}\"" : kvp.Key;
                        writer.WriteLine($"{escapedCompany},{kvp.Value}");
                    }
                }
                Console.WriteLine($"Contact summary written to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write CSV file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
