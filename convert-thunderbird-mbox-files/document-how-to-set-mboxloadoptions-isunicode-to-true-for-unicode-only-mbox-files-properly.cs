using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "unicode.mbox";

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            // Load options for MBOX (Unicode handling is automatic in recent versions)
            var loadOptions = new MboxLoadOptions();

            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                int index = 0;
                MailMessage message;
                while ((message = mboxReader.ReadNextMessage()) != null)
                {
                    index++;

                    Console.WriteLine($"Message #{index}");
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.From}");
                    Console.WriteLine($"To: {message.To}");

                    // Create a safe filename
                    string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                    safeSubject = Regex.Replace(safeSubject, @"[\\/:*?""<>|]", "_");
                    string fileName = $"{index:D4}_{safeSubject}.eml";
                    string outputPath = Path.Combine(outputDir, fileName);

                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Saved: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message #{index}: {ex.Message}");
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
