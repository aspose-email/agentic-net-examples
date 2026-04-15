using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are detected.
            if (mailboxUri.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase) || password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Ensure the output directory exists.
            string outputDir = "SortedContacts";
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Connect to Exchange and retrieve contacts.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Retrieve contacts from the default "Contacts" folder.
                    Contact[] contacts = client.GetContacts("Contacts");

                    // Sort contacts by last name (Surname) alphabetically.
                    List<Contact> sortedContacts = contacts
                        .OrderBy(c => c.Surname ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                        .ThenBy(c => c.GivenName ?? string.Empty, StringComparer.OrdinalIgnoreCase)
                        .ToList();

                    // Save each contact to a VCF file in the sorted order.
                    foreach (Contact contact in sortedContacts)
                    {
                        // Build a safe file name.
                        string safeSurname = string.IsNullOrWhiteSpace(contact.Surname) ? "UnknownSurname" : contact.Surname;
                        string safeGiven = string.IsNullOrWhiteSpace(contact.GivenName) ? "UnknownGiven" : contact.GivenName;
                        string fileName = $"{safeSurname}_{safeGiven}.vcf";
                        string filePath = Path.Combine(outputDir, fileName);

                        try
                        {
                            contact.Save(filePath);
                            Console.WriteLine($"Saved contact: {fileName}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save contact '{contact.FileAs}': {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Exchange operations: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
