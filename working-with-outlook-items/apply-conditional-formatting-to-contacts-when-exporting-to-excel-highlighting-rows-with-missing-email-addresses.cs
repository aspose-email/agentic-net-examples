using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string exchangeUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls
            if (exchangeUri.Contains("example.com") ||
                username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder Exchange connection details detected. Skipping execution.");
                return;
            }

            // Output CSV file path
            string outputPath = "Contacts.csv";

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Create Exchange client and fetch contacts
            using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
            {
                List<Contact> contacts = new List<Contact>();
                try
                {
                    // Retrieve contacts from the default contacts folder
                    Contact[] fetched = client.GetContacts("contacts");
                    if (fetched != null)
                    {
                        contacts.AddRange(fetched);
                    }
                }
                catch (Exception fetchEx)
                {
                    Console.Error.WriteLine($"Failed to retrieve contacts: {fetchEx.Message}");
                    return;
                }

                // Write contacts to CSV, marking rows with missing email
                try
                {
                    using (var writer = new StreamWriter(outputPath))
                    {
                        // Header
                        writer.WriteLine("Display Name,Email Address,Missing Email");

                        foreach (Contact contact in contacts)
                        {
                            string displayName = contact.DisplayName ?? string.Empty;
                            string email = string.Empty;
                            if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                            {
                                email = contact.EmailAddresses[0].Address ?? string.Empty;
                            }

                            string missingFlag = string.IsNullOrWhiteSpace(email) ? "YES" : "NO";

                            // Escape commas in fields
                            displayName = EscapeCsv(displayName);
                            email = EscapeCsv(email);

                            writer.WriteLine($"{displayName},{email},{missingFlag}");
                        }
                    }

                    Console.WriteLine($"Contacts exported successfully to '{outputPath}'. Rows with missing email are marked with 'YES' in the 'Missing Email' column.");
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Failed to write CSV file: {writeEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Simple CSV field escaper
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
