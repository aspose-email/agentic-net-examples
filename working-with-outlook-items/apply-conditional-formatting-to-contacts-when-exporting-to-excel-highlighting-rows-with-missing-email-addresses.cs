using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define the output CSV file path
            string outputPath = "contacts.csv";

            // Ensure the directory for the output file exists
            try
            {
                string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Prepare a list of contacts (some with missing email addresses)
            List<Contact> contacts = new List<Contact>();

            Contact contact1 = new Contact
            {
                GivenName = "John",
                Surname = "Doe",
                EmailAddresses = { new EmailAddress("john.doe@example.com") }
            };
            contacts.Add(contact1);

            Contact contact2 = new Contact
            {
                GivenName = "Jane",
                Surname = "Smith"
                // No email address added
            };
            contacts.Add(contact2);

            Contact contact3 = new Contact
            {
                GivenName = "Bob",
                Surname = "Brown",
                EmailAddresses = { new EmailAddress("bob.brown@example.com") }
            };
            contacts.Add(contact3);

            // Export contacts to CSV
            try
            {
                using (StreamWriter writer = new StreamWriter(outputPath, false))
                {
                    // Write CSV header
                    writer.WriteLine("GivenName,Surname,Email");

                    // Write each contact row
                    foreach (Contact c in contacts)
                    {
                        string email = c.EmailAddresses.Count > 0 ? c.EmailAddresses[0].Address : string.Empty;
                        // In Excel you can apply conditional formatting on the "Email" column
                        // to highlight rows where this field is empty.
                        writer.WriteLine($"{EscapeCsv(c.GivenName)},{EscapeCsv(c.Surname)},{EscapeCsv(email)}");
                    }
                }

                Console.WriteLine($"Contacts exported successfully to '{outputPath}'.");
                Console.WriteLine("To highlight rows with missing email addresses, open the CSV in Excel");
                Console.WriteLine("and apply a conditional formatting rule on the Email column (e.g.,");
                Console.WriteLine("Use a formula like =LEN(TRIM(C2))=0 to format rows where the email is empty).");
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Failed to write contacts to file: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper method to escape CSV fields containing commas or quotes
    private static string EscapeCsv(string field)
    {
        if (field == null)
            return string.Empty;

        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            string escaped = field.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
        }
        return field;
    }
}
