using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";
            string contactsFolder = "Contacts";
            string companyFilter = "Contoso";
            string outputCsv = "filtered_contacts.csv";

            // Skip external call if placeholder credentials are detected
            if (exchangeUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputCsv);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDir}': {ex.Message}");
                    return;
                }
            }

            // Connect to Exchange and retrieve contacts
            try
            {
                using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                {
                    Contact[] allContacts = client.GetContacts(contactsFolder);
                    List<Contact> filtered = new List<Contact>();
                    foreach (Contact contact in allContacts)
                    {
                        if (contact.CompanyName != null && contact.CompanyName.Equals(companyFilter, StringComparison.OrdinalIgnoreCase))
                        {
                            filtered.Add(contact);
                        }
                    }

                    // Write filtered contacts to CSV
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(outputCsv, false))
                        {
                            // Header
                            writer.WriteLine("DisplayName,EmailAddress,CompanyName");
                            foreach (Contact contact in filtered)
                            {
                                string displayName = contact.DisplayName?.Replace(",", " ");
                                string email = contact.EmailAddresses?.Count > 0 ? contact.EmailAddresses[0].Address : "";
                                string company = contact.CompanyName?.Replace(",", " ");
                                writer.WriteLine($"{displayName},{email},{company}");
                            }
                        }
                        Console.WriteLine($"Filtered contacts saved to '{outputCsv}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error writing CSV file: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
