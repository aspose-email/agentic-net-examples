using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define output directory for the .eml files
            string outputDirectory = "Emails";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Loop to create and save 50 email messages
            for (int i = 1; i <= 50; i++)
            {
                // Construct unique recipient and subject
                string recipientAddress = $"recipient{i}@example.com";
                string subjectText = $"Test Email {i}";
                string bodyText = $"This is the body of email {i}.";

                // Create the email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress("sender@example.com");
                    message.To.Add(new MailAddress(recipientAddress));
                    message.Subject = subjectText;
                    message.Body = bodyText;

                    // Determine file path
                    string emlPath = Path.Combine(outputDirectory, $"email_{i}.eml");

                    // Save the message as .eml with error handling
                    try
                    {
                        message.Save(emlPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save email {i}: {ex.Message}");
                        // Continue with next message
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
