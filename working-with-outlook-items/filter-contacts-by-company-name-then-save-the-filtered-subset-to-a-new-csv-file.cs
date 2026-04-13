using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string outputCsvPath = "filtered_contacts.csv";

            // Prepare sample contacts (replace with real source in production)
            List<Contact> contacts = new List<Contact>();

            Contact contact1 = new Contact();
            contact1.GivenName = "John";
            contact1.Surname = "Doe";
            contact1.CompanyName = "Acme Corp";
            contacts.Add(contact1);

            Contact contact2 = new Contact();
            contact2.GivenName = "Jane";
            contact2.Surname = "Smith";
            contact2.CompanyName = "Globex Inc";
            contacts.Add(contact2);

            Contact contact3 = new Contact();
            contact3.GivenName = "Alice";
            contact3.Surname = "Brown";
            contact3.CompanyName = "Acme Corp";
            contacts.Add(contact3);

            // Filter contacts by company name
            List<Contact> filteredContacts = contacts
                .Where(c => string.Equals(c.CompanyName, "Acme Corp", StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputCsvPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Write filtered contacts to CSV
            using (StreamWriter writer = new StreamWriter(outputCsvPath, false))
            {
                // Write CSV header
                writer.WriteLine("GivenName,Surname,CompanyName");

                // Write each filtered contact
                foreach (Contact filteredContact in filteredContacts)
                {
                    string line = string.Format("{0},{1},{2}",
                        EscapeCsv(filteredContact.GivenName),
                        EscapeCsv(filteredContact.Surname),
                        EscapeCsv(filteredContact.CompanyName));
                    writer.WriteLine(line);
                }
            }

            Console.WriteLine("Filtered contacts have been saved to: " + outputCsvPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }

    // Helper method to escape CSV fields
    private static string EscapeCsv(string field)
    {
        if (field == null)
            return string.Empty;

        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            string escaped = field.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }
        else
        {
            return field;
        }
    }
}
