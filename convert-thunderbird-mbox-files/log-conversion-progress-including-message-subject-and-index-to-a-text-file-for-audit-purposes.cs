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
            const string mboxPath = "storage.mbox";
            const string outputDir = "output";
            const string logFileName = "conversion_log.txt";
            string logPath = Path.Combine(outputDir, logFileName);

            // Verify the MBOX file exists.
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {mboxPath}");
                return;
            }

            // Ensure the output directory exists.
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Ensure the log file exists (create if missing).
            try
            {
                using (FileStream fs = new FileStream(logPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    // No action needed; just ensure the file is created.
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create log file: {ex.Message}");
                return;
            }

            // Open the MBOX storage.
            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                int index = 0;

                // Open the log file for appending.
                using (StreamWriter logWriter = new StreamWriter(logPath, append: true))
                {
                    // Iterate through each message info object.
                    foreach (MboxMessageInfo mboxMessageInfo in mbox.EnumerateMessageInfo())
                    {
                        index++;

                        // Log progress.
                        logWriter.WriteLine($"Index: {index}, Subject: {mboxMessageInfo.Subject}");

                        // Extract the full MIME message.
                        MailMessage eml = mbox.ExtractMessage(mboxMessageInfo.EntryId, new EmlLoadOptions());

                        // Sanitize filename (remove invalid characters).
                        string safeSubject = string.IsNullOrWhiteSpace(eml.Subject) ? $"Message_{index}" : eml.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        string emlFileName = $"{safeSubject}.eml";
                        string emlPath = Path.Combine(outputDir, emlFileName);

                        // Save the extracted message.
                        eml.Save(emlPath);
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
