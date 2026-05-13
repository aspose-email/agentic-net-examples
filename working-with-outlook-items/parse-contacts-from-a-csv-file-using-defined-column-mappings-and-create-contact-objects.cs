using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the CSV file
            string csvPath = "contacts.csv";

            // Ensure the CSV file exists; create a minimal placeholder if it does not
            if (!File.Exists(csvPath))
            {
                var placeholderLines = new[]
                {
                    "DisplayName,GivenName,Surname,Email",
                    "John Doe,John,Doe,john.doe@example.com",
                    "Jane Smith,Jane,Smith,jane.smith@example.com"
                };
                File.WriteAllLines(csvPath, placeholderLines);
                Console.WriteLine($"Placeholder CSV file created at: {csvPath}");
            }

            // List to hold the created contacts
            List<Contact> contacts = new List<Contact>();

            // Read and parse the CSV file
            using (FileStream fileStream = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                // Read header line and build column index map
                string headerLine = reader.ReadLine();
                if (headerLine == null)
                {
                    Console.Error.WriteLine("CSV file is empty.");
                    return;
                }

                string[] headers = headerLine.Split(',');
                Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < headers.Length; i++)
                {
                    columnMap[headers[i].Trim()] = i;
                }

                // Expected column names (adjust as needed)
                const string displayNameColumn = "DisplayName";
                const string givenNameColumn = "GivenName";
                const string surnameColumn = "Surname";
                const string emailColumn = "Email";

                // Read each data line
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] fields = line.Split(',');

                    Contact contact = new Contact();

                    // Map DisplayName
                    if (columnMap.TryGetValue(displayNameColumn, out int displayNameIdx) && displayNameIdx < fields.Length)
                        contact.DisplayName = fields[displayNameIdx].Trim();

                    // Map GivenName
                    if (columnMap.TryGetValue(givenNameColumn, out int givenNameIdx) && givenNameIdx < fields.Length)
                        contact.GivenName = fields[givenNameIdx].Trim();

                    // Map Surname
                    if (columnMap.TryGetValue(surnameColumn, out int surnameIdx) && surnameIdx < fields.Length)
                        contact.Surname = fields[surnameIdx].Trim();

                    // Map Email (adds to EmailAddresses collection)
                    if (columnMap.TryGetValue(emailColumn, out int emailIdx) && emailIdx < fields.Length)
                    {
                        string email = fields[emailIdx].Trim();
                        if (!string.IsNullOrEmpty(email))
                        {
                            contact.EmailAddresses.Add(new EmailAddress(email));
                        }
                    }

                    contacts.Add(contact);
                }
            }

            // Output the created contacts
            foreach (Contact c in contacts)
            {
                string email = c.EmailAddresses.Count > 0 ? c.EmailAddresses[0].Address : "N/A";
                Console.WriteLine($"Contact: {c.DisplayName}, Email: {email}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
