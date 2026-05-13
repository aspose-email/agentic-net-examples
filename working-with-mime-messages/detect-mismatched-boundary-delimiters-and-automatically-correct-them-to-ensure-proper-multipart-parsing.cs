using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.eml";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    using (StreamWriter writer = new StreamWriter(inputPath))
                    {
                        writer.WriteLine("From: sender@example.com");
                        writer.WriteLine("To: recipient@example.com");
                        writer.WriteLine("Subject: Placeholder");
                        writer.WriteLine("MIME-Version: 1.0");
                        writer.WriteLine("Content-Type: multipart/mixed; boundary=\"boundary123\"");
                        writer.WriteLine();
                        writer.WriteLine("--boundary123");
                        writer.WriteLine("Content-Type: text/plain; charset=\"utf-8\"");
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder body.");
                        writer.WriteLine("--boundary123--");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the message, which forces Aspose.Email to parse the MIME structure.
            try
            {
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    // Save the message; Aspose.Email will regenerate correct boundary delimiters.
                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved with corrected boundaries to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save corrected message: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
