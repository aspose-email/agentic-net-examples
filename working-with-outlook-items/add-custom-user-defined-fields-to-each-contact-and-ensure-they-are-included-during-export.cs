using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip execution if not replaced with real values
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create Exchange client (connection safety wrapped in try/catch)
            try
            {
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Client created successfully – no server operations needed for this example
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to connect to Exchange server: {ex.Message}");
                return;
            }

            // Prepare output directory for exported contacts
            string outputDir = Path.Combine(Environment.CurrentDirectory, "ExportedContacts");
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

            // Create contacts with custom user‑defined fields
            List<MapiContact> contacts = new List<MapiContact>();
            for (int i = 1; i <= 2; i++)
            {
                MapiContact mapiContact = new MapiContact();
                mapiContact.NameInfo.GivenName = $"John{i}";
                mapiContact.NameInfo.Surname = $"Doe{i}";
                mapiContact.NameInfo.DisplayName = $"John{i} Doe{i}";

                // Set custom user‑defined fields (UserField1‑4)
                mapiContact.OtherFields.UserField1 = $"CustomField1_Value{i}";
                mapiContact.OtherFields.UserField2 = $"CustomField2_Value{i}";
                mapiContact.OtherFields.UserField3 = $"CustomField3_Value{i}";
                mapiContact.OtherFields.UserField4 = $"CustomField4_Value{i}";

                contacts.Add(mapiContact);
            }

            // Export each contact to a VCard file, ensuring custom fields are included
            foreach (MapiContact contact in contacts)
            {
                using (contact)
                {
                    string fileName = $"{contact.NameInfo.DisplayName.Replace(' ', '_')}.vcf";
                    string filePath = Path.Combine(outputDir, fileName);
                    try
                    {
                        contact.Save(filePath);
                        Console.WriteLine($"Exported contact to: {filePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to export contact '{contact.NameInfo.DisplayName}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
