using System;
using System.IO;
using Aspose.Email;

namespace ExtractPlainTextBody
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputPath = "sample.eml";
                string outputPath = "plain_body.txt";

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
                        using (StreamWriter placeholderWriter = new StreamWriter(inputPath))
                        {
                            placeholderWriter.WriteLine("From: example@example.com");
                            placeholderWriter.WriteLine("To: recipient@example.com");
                            placeholderWriter.WriteLine("Subject: Placeholder Email");
                            placeholderWriter.WriteLine();
                            placeholderWriter.WriteLine("This is a placeholder plain text body.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                        return;
                    }
                }

                // Load the email message.
                MailMessage mailMessage;
                try
                {
                    mailMessage = MailMessage.Load(inputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load email message: {ex.Message}");
                    return;
                }

                using (mailMessage)
                {
                    string plainBody = mailMessage.Body ?? string.Empty;

                    // Save the plain‑text body to a .txt file.
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(outputPath, false))
                        {
                            writer.Write(plainBody);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write plain text body to file: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
