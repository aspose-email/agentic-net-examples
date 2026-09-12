using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MBOX file
            string mboxPath = "storage.mbox";

            // Verify that the MBOX file exists before proceeding
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Configure load options with the desired text encoding (UTF-8 in this example)
            var loadOptions = new MboxLoadOptions
            {
                PreferredTextEncoding = Encoding.UTF8
            };

            // Ensure output directory exists
            string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            // Create a reader for the MBOX storage using the specified options
            using (var mboxReader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                int messageIndex = 0;
                MailMessage message;
                while ((message = mboxReader.ReadNextMessage()) != null)
                {
                    using (message)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"To: {message.To}");

                        // Create a safe filename based on the message subject
                        string safeSubject = string.IsNullOrEmpty(message.Subject)
                            ? $"NoSubject_{messageIndex}"
                            : message.Subject;

                        foreach (char invalidChar in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(invalidChar, '_');
                        }

                        string outputPath = Path.Combine(outputDir, $"{safeSubject}.eml");

                        // Save the extracted message as an .eml file, handling any I/O errors
                        try
                        {
                            message.Save(outputPath);
                            Console.WriteLine($"Saved: {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                        }

                        messageIndex++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
