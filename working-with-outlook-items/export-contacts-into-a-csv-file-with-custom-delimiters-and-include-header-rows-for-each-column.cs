using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

namespace ExportContactsToCsv
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define output CSV file path
                string outputFilePath = "contacts.csv";
                string outputDirectory = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Prepare sample contacts
                List<Contact> contacts = new List<Contact>();

                Contact contact1 = new Contact();
                contact1.GivenName = "John";
                contact1.Surname = "Doe";
                contact1.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));
                contact1.PhoneNumbers.Add(new PhoneNumber { Number = "+1234567890", Category = PhoneNumberCategory.Company });
                contacts.Add(contact1);

                Contact contact2 = new Contact();
                contact2.GivenName = "Jane";
                contact2.Surname = "Smith";
                contact2.EmailAddresses.Add(new EmailAddress("jane.smith@example.com"));
                contact2.PhoneNumbers.Add(new PhoneNumber { Number = "+1987654321", Category = PhoneNumberCategory.Company });
                contacts.Add(contact2);

                // Custom delimiter for CSV
                string delimiter = "|";

                // Write contacts to CSV with header row
                try
                {
                    using (StreamWriter writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
                    {
                        // Header row
                        writer.WriteLine(string.Join(delimiter, "GivenName", "Surname", "Email", "Phone"));

                        // Data rows
                        foreach (Contact c in contacts)
                        {
                            string email = string.Empty;
                            if (c.EmailAddresses.Count > 0)
                            {
                                email = c.EmailAddresses[0].Address;
                            }

                            string phone = string.Empty;
                            if (c.PhoneNumbers.Count > 0)
                            {
                                phone = c.PhoneNumbers[0].Number;
                            }

                            writer.WriteLine(string.Join(delimiter,
                                c.GivenName ?? string.Empty,
                                c.Surname ?? string.Empty,
                                email,
                                phone));
                        }
                    }
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Error writing CSV file: {ioEx.Message}");
                    return;
                }

                Console.WriteLine($"Contacts exported successfully to '{outputFilePath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
