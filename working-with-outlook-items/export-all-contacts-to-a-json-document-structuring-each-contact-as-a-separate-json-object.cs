using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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

            // Skip execution when placeholders are detected.
            if (string.IsNullOrWhiteSpace(clientId) || clientId.Contains("your-"))
            {
                Console.Error.WriteLine("Client credentials are placeholders. Skipping execution.");
                return;
            }

            // Create Gmail client (implements IDisposable). Pass null for proxy.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, null, clientSecret, refreshToken))
            {
                // Retrieve all contacts.
                Contact[] contacts = gmailClient.GetAllContacts();

                // Convert contacts to a list of simple dictionaries for JSON serialization.
                List<Dictionary<string, string>> jsonContacts = new List<Dictionary<string, string>>();
                foreach (Contact contact in contacts)
                {
                    var dict = new Dictionary<string, string>
                    {
                        { "DisplayName", contact.DisplayName ?? string.Empty },
                        { "GivenName", contact.GivenName ?? string.Empty },
                        { "Surname", contact.Surname ?? string.Empty },
                        { "CompanyName", contact.CompanyName ?? string.Empty },
                        { "JobTitle", contact.JobTitle ?? string.Empty }
                    };
                    jsonContacts.Add(dict);
                }

                string outputPath = "contacts.json";

                // Ensure the output directory exists and write the JSON file.
                try
                {
                    string directory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    string json = JsonSerializer.Serialize(jsonContacts, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(outputPath, json);
                    Console.WriteLine($"Exported {jsonContacts.Count} contacts to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"File operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
