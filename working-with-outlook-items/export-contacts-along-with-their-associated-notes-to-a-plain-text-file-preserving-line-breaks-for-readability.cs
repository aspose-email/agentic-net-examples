using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Output text file path
            string outputPath = "contacts.txt";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Placeholder Gmail credentials (replace with real values)
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            // Skip execution when placeholder credentials are detected
            if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") || refreshToken.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder Gmail credentials detected. Skipping contact export.");
                return;
            }

            // Create Gmail client (proxy parameter set to null)
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, null);

            // Retrieve all contacts
            Contact[] contacts = gmailClient.GetAllContacts();

            // Write contacts and their notes to the text file
            using (StreamWriter writer = new StreamWriter(outputPath, false))
            {
                foreach (Contact contact in contacts)
                {
                    string identifier = contact.FileAs ?? string.Empty;
                    writer.WriteLine($"Contact: {identifier}");
                    writer.WriteLine("Notes:");
                    writer.WriteLine(contact.Notes ?? string.Empty);
                    writer.WriteLine(new string('-', 40));
                }
            }

            Console.WriteLine($"Exported {contacts.Length} contacts to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
