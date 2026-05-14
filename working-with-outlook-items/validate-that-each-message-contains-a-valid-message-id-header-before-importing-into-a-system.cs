using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            const string inputFolder = "InputMessages";

            // Ensure the input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                return;
            }

            string[] emlFiles;
            try
            {
                emlFiles = Directory.GetFiles(inputFolder, "*.eml");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                return;
            }

            if (emlFiles.Length == 0)
            {
                Console.WriteLine("No EML files found to process.");
                return;
            }

            foreach (string emlPath in emlFiles)
            {
                // Guard file existence
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

                    Console.Error.WriteLine($"File not found: {emlPath}");
                    continue;
                }

                try
                {
                    using (MailMessage message = MailMessage.Load(emlPath))
                    {
                        // Check for Message-ID header
                        string messageId = message.Headers["Message-ID"];
                        if (string.IsNullOrEmpty(messageId))
                        {
                            Console.Error.WriteLine($"Message-ID header missing in file: {emlPath}");
                            // Handle the case as needed (e.g., skip import)
                            continue;
                        }

                        // At this point the message is considered valid for import
                        Console.WriteLine($"Valid message '{messageId}' loaded from '{emlPath}'.");
                        // Insert import logic here
                    }
                }
                catch (AsposeException aex)
                {
                    Console.Error.WriteLine($"Aspose error processing '{emlPath}': {aex.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error processing '{emlPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
