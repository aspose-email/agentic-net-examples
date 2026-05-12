using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.mime";
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
                    using (FileStream placeholderStream = File.Create(inputPath))
                    using (StreamWriter writer = new StreamWriter(placeholderStream))
                    {
                        writer.WriteLine("From: placeholder@example.com");
                        writer.WriteLine("To: recipient@example.com");
                        writer.WriteLine("Subject: Placeholder");
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder MIME message.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder input file: {ex.Message}");
                    return;
                }
            }

            // Load the MIME message and save it as EML.
            try
            {
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    message.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing email: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
