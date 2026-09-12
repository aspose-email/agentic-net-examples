using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "username@example.com";
            string password = "password";
            string contactsFolderUri = "contacts"; // Adjust as needed
            string outputCsvPath = "ExportedContacts/contacts.csv";

            // Skip execution when placeholders are detected
            if (mailboxUri.Contains("example") || username.Contains("example") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputCsvPath);
            if (string.IsNullOrEmpty(outputDirectory))
                outputDirectory = Directory.GetCurrentDirectory();

            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Create and connect the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, new NetworkCredential(username, password)))
            {
                try
                {
                    // Retrieve contacts from the specified folder
                    MapiContact[] contacts = client.ListContacts(contactsFolderUri);
                    if (contacts == null || contacts.Length == 0)
                    {
                        Console.WriteLine("No contacts found in the specified folder.");
                        return;
                    }

                    // Prepare CSV writer
                    using (StreamWriter csvWriter = new StreamWriter(outputCsvPath, false, Encoding.UTF8))
                    {
                        // Write CSV header
                        csvWriter.WriteLine("DisplayName,Email1,PhotoFileName,CustomFields");

                        int contactIndex = 0;
                        foreach (MapiContact contact in contacts)
                        {
                            contactIndex++;

                            // Basic fields
                            string displayName = contact.NameInfo?.DisplayName ?? string.Empty;
                            string email1 = contact.ElectronicAddresses?.Email1?.EmailAddress ?? string.Empty;

                            // Save photo if present
                            string photoFileName = string.Empty;
                            if (contact.Photo?.Data != null && contact.Photo.Data.Length > 0)
                            {
                                photoFileName = $"photo_{contactIndex}.jpg";
                                string photoPath = Path.Combine(outputDirectory, photoFileName);
                                try
                                {
                                    File.WriteAllBytes(photoPath, contact.Photo.Data);
                                }
                                catch (Exception photoEx)
                                {
                                    Console.Error.WriteLine($"Failed to write photo for contact {displayName}: {photoEx.Message}");
                                    photoFileName = string.Empty;
                                }
                            }

                            // Gather custom (named) properties
                            List<string> customPairs = new List<string>();
                            foreach (KeyValuePair<long, MapiProperty> kvp in contact.NamedProperties)
                            {
                                MapiProperty prop = kvp.Value;
                                try
                                {
                                    string propName = prop.Name ?? kvp.Key.ToString("X");
                                    string propValue = prop.GetValue()?.ToString() ?? string.Empty;
                                    customPairs.Add($"{propName}={propValue}");
                                }
                                catch
                                {
                                    // Ignore properties that cannot be read as string
                                }
                            }
                            string customFields = string.Join(";", customPairs);

                            // Escape fields for CSV
                            string Escape(string s) => $"\"{s.Replace("\"", "\"\"")}\"";

                            // Write CSV line
                            csvWriter.WriteLine($"{Escape(displayName)},{Escape(email1)},{Escape(photoFileName)},{Escape(customFields)}");
                        }
                    }

                    Console.WriteLine($"Contacts exported successfully to '{outputCsvPath}'.");
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"Error during Exchange operation: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
