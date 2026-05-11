using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the .eml file containing the HTML email
            string emlPath = "sample.eml";

            // Verify that the email file exists before attempting to load it
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Email file not found: {emlPath}");
                return;
            }

            // Load the email message from the .eml file
            MailMessage emailMessage;
            try
            {
                emailMessage = MailMessage.Load(emlPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load email: {loadEx.Message}");
                return;
            }

            // Extract plain‑text from the HTML body (without URLs)
            string plainTextBody = emailMessage.GetHtmlBodyText(false);

            // Store the extracted plain‑text body into the database (placeholder implementation)
            StorePlainText(emailMessage.MessageId, plainTextBody);

            Console.WriteLine("Plain‑text body processed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Placeholder method representing storage of the plain‑text body.
    // Replace with actual database logic as needed.
    static void StorePlainText(string messageId, string plainTextBody)
    {
        // Example: write to console; in real scenario, insert into DB.
        Console.WriteLine($"MessageId: {messageId ?? "N/A"}");
        Console.WriteLine("Plain‑text Body:");
        Console.WriteLine(plainTextBody ?? string.Empty);
    }
}
