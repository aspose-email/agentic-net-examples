using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Bounce;

class Program
{
    static void Main()
    {
        try
        {
            string originalPath = "original.eml";
            string bouncePath = "bounce.eml";

            // Ensure the original message file exists; create a minimal placeholder if missing.
            if (!File.Exists(originalPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(originalPath, SaveOptions.DefaultEml);
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
            }

            // Load the original message.
            MailMessage originalMessage;
            try
            {
                originalMessage = MailMessage.Load(originalPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load original message: {ex.Message}");
                return;
            }

            using (originalMessage)
            {
                // Check if the message is a bounce.
                BounceResult bounceResult = originalMessage.CheckBounced();

                if (!bounceResult.IsBounced)
                {
                    Console.WriteLine("The loaded message is not a bounce message.");
                    return;
                }

                // Create a new bounce (undeliverable) message.
                using (MailMessage bounceMessage = new MailMessage())
                {
                    // Set sender as postmaster.
                    bounceMessage.From = new MailAddress("postmaster@example.com", "Postmaster");

                    // Set recipient to the original sender.
                    bounceMessage.To.Add(originalMessage.From);

                    // Subject indicating undeliverable.
                    bounceMessage.Subject = "Undeliverable: " + originalMessage.Subject;

                    // Build the body with the failure reason and original content.
                    string body = $"Your message could not be delivered.\n\nReason: {bounceResult.Reason}\nStatus: {bounceResult.Status}\n\n--- Original Message ---\n{originalMessage.Body}";
                    bounceMessage.Body = body;

                    // Attach the original message for reference.
                    try
                    {
                        bounceMessage.Attachments.Add(new Attachment(originalPath));
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to attach original message: {ex.Message}");
                    }

                    // Save the bounce message.
                    try
                    {
                        bounceMessage.Save(bouncePath);
                        Console.WriteLine($"Bounce message saved to '{bouncePath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save bounce message: {ex.Message}");
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
