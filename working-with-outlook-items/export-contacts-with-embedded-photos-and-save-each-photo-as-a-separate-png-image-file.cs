using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            // Guard against placeholder credentials.
            if (string.IsNullOrWhiteSpace(clientId) ||
                string.IsNullOrWhiteSpace(clientSecret) ||
                string.IsNullOrWhiteSpace(refreshToken) ||
                clientId.Contains("your") ||
                clientSecret.Contains("your") ||
                refreshToken.Contains("your"))
            {
                Console.Error.WriteLine("Please provide valid Gmail API credentials before running the sample.");
                return;
            }

            // Create Gmail client. Pass null for proxy (default).
            IGmailClient client;
            try
            {
                client = GmailClient.GetInstance(clientId, null, clientSecret, refreshToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Fetch all contacts.
            Contact[] contacts;
            try
            {
                contacts = client.GetAllContacts();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to retrieve contacts: {ex.Message}");
                return;
            }

            // Prepare output folder.
            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "ContactPhotos");
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory '{outputFolder}': {ex.Message}");
                return;
            }

            // Export each contact's photo.
            for (int i = 0; i < contacts.Length; i++)
            {
                Contact contact = contacts[i];
                byte[] photoData = contact.Photo?.Data;

                if (photoData == null || photoData.Length == 0)
                {
                    // No photo for this contact.
                    continue;
                }

                // Determine file name.
                string fileName;
                if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                {
                    // Use first email address as file name.
                    string email = contact.EmailAddresses[0].Address;
                    // Sanitize file name characters.
                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        email = email.Replace(c, '_');
                    }
                    fileName = $"{email}.png";
                }
                else
                {
                    fileName = $"Contact_{i + 1}.png";
                }

                string filePath = Path.Combine(outputFolder, fileName);

                // Write photo bytes to PNG file.
                try
                {
                    File.WriteAllBytes(filePath, photoData);
                    Console.WriteLine($"Saved photo for contact '{contact.DisplayName}' to '{filePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save photo for contact '{contact.DisplayName}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
