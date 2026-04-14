using Aspose.Email.Storage.Pst;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Connection settings
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Public folder name that contains contacts
            string publicFolderName = "ContactsPublicFolder";

            // Output CSV file
            string csvPath = "contacts.csv";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Ensure output directory exists
            string csvDir = Path.GetDirectoryName(csvPath);
            if (!string.IsNullOrEmpty(csvDir) && !Directory.Exists(csvDir))
            {
                Directory.CreateDirectory(csvDir);
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Locate the public folder by name
                ExchangeFolderInfoCollection publicFolders = client.ListPublicFolders();
                string targetFolderUri = null;
                foreach (ExchangeFolderInfo folder in publicFolders)
                {
                    if (folder.DisplayName.Equals(publicFolderName, StringComparison.OrdinalIgnoreCase))
                    {
                        targetFolderUri = folder.Uri;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(targetFolderUri))
                {
                    Console.Error.WriteLine("Public folder not found: " + publicFolderName);
                    return;
                }

                // Retrieve contacts from the public folder
                Contact[] contacts = client.GetContacts(targetFolderUri);

                // Export contacts to CSV
                try
                {
                    using (StreamWriter writer = new StreamWriter(csvPath, false, Encoding.UTF8))
                    {
                        writer.WriteLine("DisplayName,EmailAddress");
                        foreach (Contact contact in contacts)
                        {
                            string displayName = contact.DisplayName ?? string.Empty;
                            string email = string.Empty;
                            if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                            {
                                email = contact.EmailAddresses[0].Address ?? string.Empty;
                            }

                            // Escape double quotes
                            displayName = displayName.Replace("\"", "\"\"");
                            email = email.Replace("\"", "\"\"");

                            writer.WriteLine($"\"{displayName}\",\"{email}\"");
                        }
                    }
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine("Error writing CSV: " + ioEx.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
