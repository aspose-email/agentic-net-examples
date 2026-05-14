using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholders are detected.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create Exchange client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Example: create a list of contacts to export.
                List<Contact> contacts = new List<Contact>();

                Contact contact = new Contact();
                contact.DisplayName = "John Doe";
                contact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));
                contacts.Add(contact);

                // Define custom prefix.
                string prefix = "Custom_";

                // Ensure output directory exists.
                string outputDir = "ExportedContacts";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Export each contact with prefixed display name.
                foreach (Contact c in contacts)
                {
                    c.DisplayName = prefix + c.DisplayName;

                    string filePath = Path.Combine(outputDir, c.DisplayName + ".vcf");

                    try
                    {
                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            c.Save(fs);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save contact '{c.DisplayName}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
