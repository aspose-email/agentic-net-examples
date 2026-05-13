using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Simulated dropped file paths for a console sample.
            string[] droppedFiles = new string[] { "sample.eml", "sample.msg" };

            foreach (string filePath in droppedFiles)
            {
                // Ensure the directory for the file exists before any write operation.
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (!File.Exists(filePath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    try
                    {
                        Console.WriteLine($"Created placeholder file: {filePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                        return;
                    }

                    // Continue to next file after creating placeholder.
                    continue;
                }

                try
                {
                    using (MailMessage message = MailMessage.Load(filePath))
                    {
                        Console.WriteLine($"Loaded: {filePath}");
                        Console.WriteLine($"Subject: {message.Subject}");

                        // If the original file is not MSG, save a copy as MSG.
                        string extension = Path.GetExtension(filePath).ToLowerInvariant();
                        if (extension != ".msg")
                        {
                            string msgPath = Path.ChangeExtension(filePath, ".msg");

                            // Ensure the target directory exists.
                            string msgDir = Path.GetDirectoryName(msgPath);
                            if (!string.IsNullOrEmpty(msgDir) && !Directory.Exists(msgDir))
                            {
                                Directory.CreateDirectory(msgDir);
                            }

                            message.Save(msgPath, SaveOptions.DefaultMsg);
                            Console.WriteLine($"Saved as MSG: {msgPath}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{filePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
