using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            const string mboxPath = "storage.mbox";
            const string outputDir = "output";

            // Ensure the MBOX file exists.
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input file not found: '{mboxPath}'.");
                return;
            }

            // Ensure the output directory exists.
            Directory.CreateDirectory(outputDir);

            // Open the MBOX file as a read‑only stream.
            using (FileStream fileStream = new FileStream(mboxPath, FileMode.Open, FileAccess.Read))
            {
                // Create a reader with default load options.
                using (var reader = MboxStorageReader.CreateReader(fileStream, new MboxLoadOptions()))
                {
                    int index = 0;
                    while (true)
                    {
                        // Read the next message; returns null when no more messages.
                        MailMessage message = reader.ReadNextMessage();
                        if (message == null)
                            break;

                        using (message)
                        {
                            // Build a safe file name.
                            string subject = string.IsNullOrWhiteSpace(message.Subject) ? $"Message_{index}" : message.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                                subject = subject.Replace(c, '_');

                            string fileName = $"{subject}_{index}.eml";
                            string outputPath = Path.Combine(outputDir, fileName);

                            try
                            {
                                message.Save(outputPath);
                                Console.WriteLine($"Saved: {outputPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message '{fileName}': {ex.Message}");
                            }

                            index++;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
