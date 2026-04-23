using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            const string mboxPath = "input.mbox";
            const string outputDirectory = "output";

            // Verify input file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                    Directory.CreateDirectory(outputDirectory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Process messages in chunks of 2000
            const int chunkSize = 2000;
            int processedCount = 0;

            try
            {
                using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    while (true)
                    {
                        int currentChunk = 0;
                        while (currentChunk < chunkSize)
                        {
                            MailMessage message = reader.ReadNextMessage();
                            if (message == null)
                                break; // No more messages

                            using (message)
                            {
                                string safeSubject = GetSafeFileName(message.Subject);
                                string outputPath = Path.Combine(outputDirectory, $"{processedCount}_{safeSubject}.html");

                                try
                                {
                                    message.Save(outputPath, new HtmlSaveOptions());
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine($"Failed to save message #{processedCount}: {ex.Message}");
                                }
                            }

                            processedCount++;
                            currentChunk++;
                        }

                        if (currentChunk == 0) // No messages read in this iteration
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MBOX file: {ex.Message}");
                return;
            }

            Console.WriteLine($"Processed {processedCount} messages.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper to create a file‑system safe name from the subject
    private static string GetSafeFileName(string subject)
    {
        if (string.IsNullOrWhiteSpace(subject))
            return "NoSubject";

        foreach (char c in Path.GetInvalidFileNameChars())
            subject = subject.Replace(c, '_');

        // Limit length to avoid overly long file names
        return subject.Length > 50 ? subject.Substring(0, 50) : subject;
    }
}
