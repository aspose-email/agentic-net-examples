using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

// Author: Example demonstrating SmtpClient.PickupDirectoryLocation usage
class Program
{
    static void Main()
    {
        try
        {
            // Define an absolute path for the SMTP pickup directory
            string pickupDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SmtpPickup");

            // Ensure the directory exists; create if missing
            if (!Directory.Exists(pickupDirectory))
            {
                Directory.CreateDirectory(pickupDirectory);
            }

            // Instantiate the SMTP client
            using (SmtpClient client = new SmtpClient())
            {
                // Set the pickup directory location (must be an absolute path)
                client.PickupDirectoryLocation = pickupDirectory;

                // Create a simple email message
                MailMessage message = new MailMessage();
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Test Email";
                message.Body = "This email is saved to the SMTP pickup directory.";

                // Save the message as an .eml file; it will be placed in the pickup directory
                string emlFilePath = Path.Combine(pickupDirectory, "test.eml");
                message.Save(emlFilePath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
