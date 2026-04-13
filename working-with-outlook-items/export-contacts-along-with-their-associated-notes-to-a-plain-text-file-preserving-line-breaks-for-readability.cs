using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Define Exchange server connection parameters (placeholders)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials – skip execution if defaults are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            // Ensure the output directory exists
            string outputPath = "contacts.txt";
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Open the output file for writing
            using (StreamWriter writer = new StreamWriter(outputPath, false, Encoding.UTF8))
            {
                // Connect to Exchange using ExchangeClient (WebDAV)
                try
                {
                    using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                    {
                        // Retrieve contacts from the default contacts folder
                        // The folder URI for contacts can be obtained via client.GetFolderInfo or known default
                        // Here we use the well‑known folder name "contacts"
                        string contactsFolderUri = client.GetFolderInfo("contacts").Uri;

                        // List contacts in the folder
                        Contact[] contacts = client.GetContacts(contactsFolderUri);

                        foreach (Contact contact in contacts)
                        {
                            writer.WriteLine("=== Contact ===");
                            writer.WriteLine($"Display Name: {contact.DisplayName}");
                            writer.WriteLine($"Email: {(contact.EmailAddresses.Count > 0 ? contact.EmailAddresses[0].Address : "N/A")}");
                            writer.WriteLine($"Company: {contact.CompanyName}");
                            writer.WriteLine($"Job Title: {contact.JobTitle}");
                            writer.WriteLine("Notes:");
                            // Preserve line breaks in the Notes field
                            writer.WriteLine(contact.Notes ?? string.Empty);
                            writer.WriteLine(); // Extra blank line between contacts
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error accessing Exchange: {ex.Message}");
                    return;
                }
            }

            Console.WriteLine($"Contacts exported successfully to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
