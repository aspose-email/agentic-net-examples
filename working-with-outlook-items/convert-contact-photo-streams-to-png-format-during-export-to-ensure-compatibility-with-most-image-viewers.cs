using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Clients.Google;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials - replace with real values.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";

            // Guard against placeholder credentials.
            if (clientId.Contains("YOUR_") || clientSecret.Contains("YOUR_") || refreshToken.Contains("YOUR_"))
            {
                Console.Error.WriteLine("Please provide valid Gmail client credentials before running the sample.");
                return;
            }

            // Paths for the source image and the PNG output.
            string sourceImagePath = "contact_photo.jpg";
            string pngOutputPath = "contact_photo.png";

            string outputDir = Path.GetDirectoryName(pngOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Verify source image exists.
            if (!File.Exists(sourceImagePath))
            {
                Console.Error.WriteLine($"Source image file not found: {sourceImagePath}");
                return;
            }

            // Read source image bytes.
            byte[] sourceImageBytes;
            try
            {
                sourceImageBytes = File.ReadAllBytes(sourceImagePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read source image: {ex.Message}");
                return;
            }

            // For this console-only sample, we assume the source image is already in PNG format.
            byte[] pngImageBytes = sourceImageBytes;

            // Save the PNG image locally (optional verification).
            try
            {
                File.WriteAllBytes(pngOutputPath, pngImageBytes);
                Console.WriteLine($"PNG image saved to: {pngOutputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write PNG file: {ex.Message}");
                return;
            }

            // Create a simple contact.
            Contact contact = new Contact
            {
                GivenName = "John",
                Surname = "Doe"
            };

            // Create Gmail client and upload the PNG photo.
            try
            {
                using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, null, clientSecret, refreshToken))
                {
                    // Create contact photo on the server using the PNG data.
                    ContactPhoto contactPhoto = gmailClient.CreateContactPhoto(contact, pngImageBytes);

                    // Update the contact with the new photo.
                    gmailClient.UpdateContactPhoto(contactPhoto);

                    Console.WriteLine("Contact photo uploaded as PNG successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Gmail client operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
