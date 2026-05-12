using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "raw_mime.txt";

            // Ensure input file exists; create a minimal placeholder if missing
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
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the MIME message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Save the message to a memory stream to obtain raw RFC822 string
                using (MemoryStream ms = new MemoryStream())
                {
                    try
                    {
                        message.Save(ms);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message to stream: {ex.Message}");
                        return;
                    }

                    // Convert stream bytes to string
                    string rawMime = Encoding.UTF8.GetString(ms.ToArray());

                    // Output the raw MIME string to console
                    Console.WriteLine("Raw RFC822 MIME message:");
                    Console.WriteLine(rawMime);

                    // Ensure output directory exists
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Write to a file
                    try
                    {
                        File.WriteAllText(outputPath, rawMime);
                        Console.WriteLine($"Raw MIME saved to: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write raw MIME to file: {ex.Message}");
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
