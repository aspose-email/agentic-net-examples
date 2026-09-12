using System;
using System.IO;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            const string mboxFilePath = "storage.mbox";
            const string outputDir = "output";

            // Ensure the MBOX file exists; create an empty placeholder if missing.
            if (!File.Exists(mboxFilePath))
            {
                try
                {
                    File.WriteAllText(mboxFilePath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists.
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            const int maxAttempts = 3;
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxFilePath, new MboxLoadOptions()))
                    {
                        MailMessage message;
                        while ((message = mbox.ReadNextMessage()) != null)
                        {
                            using (message)
                            {
                                Console.WriteLine($"Subject: {message.Subject}");
                                Console.WriteLine($"From: {message.From}");
                                Console.WriteLine($"To: {message.To}");

                                // Prepare a safe file name.
                                string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "Untitled" : message.Subject;
                                foreach (char c in Path.GetInvalidFileNameChars())
                                    safeSubject = safeSubject.Replace(c, '_');

                                string outputPath = Path.Combine(outputDir, $"{safeSubject}.eml");

                                try
                                {
                                    message.Save(outputPath);
                                    Console.WriteLine($"Saved: {outputPath}");
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Failed to save message '{safeSubject}': {saveEx.Message}");
                                }
                            }
                        }
                    }

                    // Success – exit the retry loop.
                    break;
                }
                catch (IOException ioEx)
                {
                    // Likely a file lock; retry unless max attempts reached.
                    if (attempt == maxAttempts)
                    {
                        Console.Error.WriteLine($"Unable to read MBOX after {maxAttempts} attempts: {ioEx.Message}");
                        return;
                    }

                    // Wait briefly before retrying.
                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    // Non‑IO errors are not retriable.
                    Console.Error.WriteLine($"Error reading MBOX: {ex.Message}");
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
