using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        try
        {
            string tgzPath = "archive.tgz";
            string outputDirectory = "ExportedMessages";

            // Verify input TGZ file exists
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Input file not found: {tgzPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Read messages from the Zimbra TGZ archive
            using (TgzReader tgzReader = new TgzReader(tgzPath))
            {
                int totalItems = tgzReader.GetTotalItemsCount();
                Console.WriteLine($"Total messages in archive: {totalItems}");

                for (int i = 0; i < totalItems; i++)
                {
                    try
                    {
                        tgzReader.ReadNextMessage();
                        MailMessage message = tgzReader.CurrentMessage;
                        if (message == null)
                        {
                            continue;
                        }

                        Console.WriteLine($"--- Message {i + 1} ---");
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"To: {message.To}");

                        // Prepare a safe filename based on the subject
                        string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? $"Message_{i + 1}" : message.Subject;
                        foreach (char invalidChar in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(invalidChar, '_');
                        }

                        string emlPath = Path.Combine(outputDirectory, $"{safeSubject}.eml");
                        try
                        {
                            message.Save(emlPath);
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save message to '{emlPath}': {saveEx.Message}");
                        }
                    }
                    catch (Exception readEx)
                    {
                        Console.Error.WriteLine($"Error reading message {i + 1}: {readEx.Message}");
                    }
                }

                // Optional: export the entire archive structure
                try
                {
                    tgzReader.ExportTo(outputDirectory);
                }
                catch (Exception exportEx)
                {
                    Console.Error.WriteLine($"ExportTo failed: {exportEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
