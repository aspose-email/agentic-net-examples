using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string clientId = "your_client_id";
            string clientSecret = "your_client_secret";
            string refreshToken = "your_refresh_token";

            if (string.IsNullOrWhiteSpace(clientId) ||
                string.IsNullOrWhiteSpace(clientSecret) ||
                string.IsNullOrWhiteSpace(refreshToken) ||
                clientId == "your_client_id")
            {
                Console.Error.WriteLine("Placeholder Gmail credentials detected. Skipping contact retrieval.");
                return;
            }

            // Create Gmail client (specify null for optional userEmail and proxy parameters)
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, null, null);

            try
            {
                // Fetch all contacts
                Contact[] contacts = gmailClient.GetAllContacts();

                // Build HTML content
                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.AppendLine("<!DOCTYPE html>");
                htmlBuilder.AppendLine("<html>");
                htmlBuilder.AppendLine("<head><meta charset=\"UTF-8\"><title>Contact List</title></head>");
                htmlBuilder.AppendLine("<body>");
                htmlBuilder.AppendLine("<h1>Contact List</h1>");
                htmlBuilder.AppendLine("<ul>");

                foreach (Contact contact in contacts)
                {
                    // Each contact may have multiple email addresses
                    foreach (EmailAddress email in contact.EmailAddresses)
                    {
                        string displayName = string.IsNullOrEmpty(contact.DisplayName) ? email.Address : contact.DisplayName;
                        string mailtoLink = $"mailto:{email.Address}";
                        htmlBuilder.AppendLine($"<li>{displayName}: <a href=\"{mailtoLink}\">{email.Address}</a></li>");
                    }
                }

                htmlBuilder.AppendLine("</ul>");
                htmlBuilder.AppendLine("</body>");
                htmlBuilder.AppendLine("</html>");

                // Define output path
                string outputPath = "Contacts.html";
                string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPath));

                // Ensure directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Write HTML to file
                try
                {
                    File.WriteAllText(outputPath, htmlBuilder.ToString(), Encoding.UTF8);
                    Console.WriteLine($"HTML contact list saved to: {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to write HTML file: {ioEx.Message}");
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Failed to retrieve contacts: {clientEx.Message}");
            }
            finally
            {
                // Ensure client is disposed
                if (gmailClient is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
