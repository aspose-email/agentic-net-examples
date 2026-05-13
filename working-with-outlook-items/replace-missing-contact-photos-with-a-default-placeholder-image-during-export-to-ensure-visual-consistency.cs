using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Output directory for exported contacts
            string exportDir = "ExportedContacts";

            // Ensure the output directory exists
            if (!Directory.Exists(exportDir))
            {
                Directory.CreateDirectory(exportDir);
            }

            // Path to placeholder image
            string placeholderPath = "placeholder.png";
            byte[] placeholderImage;

            // Create a minimal placeholder image if it does not exist
            if (!File.Exists(placeholderPath))
            {
                // 1x1 pixel PNG (base64 encoded)
                string base64 = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+XK6cAAAAASUVORK5CYII=";
                placeholderImage = Convert.FromBase64String(base64);
                try
                {
                    File.WriteAllBytes(placeholderPath, placeholderImage);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write placeholder image: {ex.Message}");
                    return;
                }
            }
            else
            {
                try
                {
                    placeholderImage = File.ReadAllBytes(placeholderPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to read placeholder image: {ex.Message}");
                    return;
                }
            }

            // Sample contacts (replace with actual loading logic as needed)
            Contact[] contacts = new Contact[]
            {
                new Contact { DisplayName = "Alice" },
                new Contact { DisplayName = "Bob" }
            };

            // Add email addresses to contacts
            contacts[0].EmailAddresses.Add(new EmailAddress("alice@example.com"));
            contacts[1].EmailAddresses.Add(new EmailAddress("bob@example.com"));

            foreach (Contact contact in contacts)
            {
                // Assign placeholder photo if missing
                if (contact.Photo == null)
                {
                    ContactPhoto photo = new ContactPhoto(placeholderImage, MapiContactPhotoImageFormat.Jpeg);
                    contact.Photo = photo;
                }

                // Save each contact as a vCard file
                string filePath = Path.Combine(exportDir, $"{contact.DisplayName}.vcf");
                try
                {
                    contact.Save(filePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save contact '{contact.DisplayName}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
