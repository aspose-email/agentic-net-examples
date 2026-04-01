using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emailPath = "sample.eml";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(emailPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emailPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    MailMessage placeholder = new MailMessage();
                    placeholder.From = new MailAddress("placeholder@example.com");
                    placeholder.To = new MailAddressCollection { "recipient@example.com" };
                    placeholder.Subject = "Placeholder Subject";
                    placeholder.Body = "This is a placeholder email.";
                    placeholder.Save(emailPath, SaveOptions.DefaultEml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder email: {ex.Message}");
                    return;
                }
            }

            // Load the email and perform simple validation
            try
            {
                using (MailMessage message = MailMessage.Load(emailPath))
                {
                    if (string.IsNullOrWhiteSpace(message.Subject))
                    {
                        Console.WriteLine("Validation Warning: Email subject is empty.");
                    }
                    else
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                    }

                    if (message.To == null || message.To.Count == 0)
                    {
                        Console.WriteLine("Validation Warning: No recipients specified.");
                    }
                    else
                    {
                        Console.WriteLine($"Recipient count: {message.To.Count}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading or processing email: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
