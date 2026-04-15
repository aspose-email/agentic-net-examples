using System;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string ewsUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string outputCsvPath = "ExportedContacts.csv";


            // Skip external calls when placeholder credentials are used
            if (ewsUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputCsvPath));
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Create and connect EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, new NetworkCredential(username, password)))
                {
                    // Get contacts folder URI
                    string contactsFolderUri = client.MailboxInfo.ContactsUri;

                    // Retrieve contacts
                    Contact[] contacts = client.GetContacts(contactsFolderUri);

                    // Write contacts to CSV
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(outputCsvPath, false, Encoding.UTF8))
                        {
                            // CSV header
                            writer.WriteLine("DisplayName,Email,PhoneNumbers");

                            foreach (Contact contact in contacts)
                            {
                                // Display name
                                string displayName = contact.DisplayName?.Replace(",", " ");

                                // Primary email address (if any)
                                string email = string.Empty;
                                if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                                {
                                    // Take the first email address
                                    email = contact.EmailAddresses[0].Address?.Replace(",", " ");
                                }

                                // Concatenate phone numbers
                                string phoneNumbers = string.Empty;
                                if (contact.PhoneNumbers != null && contact.PhoneNumbers.Count > 0)
                                {
                                    var numbers = contact.PhoneNumbers
                                        .Select(p => p.Number?.Replace(",", " "))
                                        .Where(n => !string.IsNullOrEmpty(n));
                                    phoneNumbers = string.Join(";", numbers);
                                }

                                // Write CSV line
                                writer.WriteLine($"{displayName},{email},{phoneNumbers}");
                            }
                        }
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Failed to write CSV file: {ioEx.Message}");
                        return;
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"EWS client error: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
