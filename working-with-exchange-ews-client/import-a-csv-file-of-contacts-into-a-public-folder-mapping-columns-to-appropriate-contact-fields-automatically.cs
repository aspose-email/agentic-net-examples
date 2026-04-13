using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the CSV file containing contacts
            string csvPath = "contacts.csv";

            // Verify the CSV file exists
            if (!File.Exists(csvPath))
            {
                Console.Error.WriteLine($"CSV file not found: {csvPath}");
                return;
            }

            // Read all lines from the CSV
            string[] lines;
            try
            {
                lines = File.ReadAllLines(csvPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read CSV file: {ex.Message}");
                return;
            }

            if (lines.Length < 2)
            {
                Console.Error.WriteLine("CSV file does not contain data rows.");
                return;
            }

            // Parse header to map column names to indices
            string[] headers = lines[0].Split(',');
            var columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < headers.Length; i++)
            {
                columnMap[headers[i].Trim()] = i;
            }

            // Prepare a list of contacts to import
            var contacts = new List<Contact>();

            for (int row = 1; row < lines.Length; row++)
            {
                string[] fields = lines[row].Split(',');

                // Helper to safely get a field value
                string GetField(string name)
                {
                    return columnMap.TryGetValue(name, out int idx) && idx < fields.Length
                        ? fields[idx].Trim()
                        : string.Empty;
                }

                var contact = new Contact();

                // Map common fields
                string displayName = GetField("DisplayName");
                if (!string.IsNullOrEmpty(displayName))
                    contact.DisplayName = displayName;

                string givenName = GetField("GivenName");
                if (!string.IsNullOrEmpty(givenName))
                    contact.GivenName = givenName;

                string surname = GetField("Surname");
                if (!string.IsNullOrEmpty(surname))
                    contact.Surname = surname;

                string email = GetField("Email");
                if (!string.IsNullOrEmpty(email))
                    contact.EmailAddresses.Add(new EmailAddress(email));

                string company = GetField("CompanyName");
                if (!string.IsNullOrEmpty(company))
                    contact.CompanyName = company;

                // Map phone number (BusinessPhone -> Company category)
                string businessPhone = GetField("BusinessPhone");
                if (!string.IsNullOrEmpty(businessPhone))
                {
                    var phone = new PhoneNumber
                    {
                        Number = businessPhone,
                        Category = PhoneNumberCategory.Company
                    };
                    contact.PhoneNumbers.Add(phone);
                }

                contacts.Add(contact);
            }

            // EWS client connection parameters
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Connect to Exchange using IEWSClient
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Locate or create the target public folder
                    string targetFolderName = "ImportedContacts";

                    // Skip external calls when placeholder credentials are used
                    if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    ExchangeFolderInfo targetFolder = null;

                    // List existing public folders
                    ExchangeFolderInfoCollection publicFolders = client.ListPublicFolders();
                    foreach (ExchangeFolderInfo folder in publicFolders)
                    {
                        if (string.Equals(folder.DisplayName, targetFolderName, StringComparison.OrdinalIgnoreCase))
                        {
                            targetFolder = folder;
                            break;
                        }
                    }

                    // Create the folder if it does not exist
                    if (targetFolder == null)
                    {
                        // Create a public folder under the root public folder
                        targetFolder = client.CreatePublicFolder(targetFolderName, new ExchangeFolderPermissionCollection());
                    }

                    // Import each contact into the public folder
                    foreach (Contact c in contacts)
                    {
                        try
                        {
                            client.CreateContact(targetFolder.Uri, c);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create contact '{c.DisplayName}': {ex.Message}");
                        }
                    }

                    Console.WriteLine("Contact import completed.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to connect to Exchange server: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
