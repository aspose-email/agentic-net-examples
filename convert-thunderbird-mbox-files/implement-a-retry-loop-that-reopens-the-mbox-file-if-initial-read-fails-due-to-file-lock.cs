using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
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

            // Ensure the MBOX file exists; create an empty placeholder if missing.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unable to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists.
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            const int maxRetries = 3;
            const int retryDelayMs = 2000;
            bool opened = false;

            for (int attempt = 1; attempt <= maxRetries && !opened; attempt++)
            {
                try
                {
                    using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                    {
                        while (true)
                        {
                            MailMessage message = mbox.ReadNextMessage();
                            if (message == null)
                                break;

                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"To: {message.To}");

                            // Create a safe file name.
                            string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "No_Subject" : message.Subject;
                            safeSubject = Regex.Replace(safeSubject, @"[\\/:*?""<>|]", "_");
                            string emlPath = Path.Combine(outputDir, $"{safeSubject}.eml");

                            try
                            {
                                message.Save(emlPath);
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save message '{message.Subject}': {saveEx.Message}");
                            }
                        }
                    }

                    opened = true; // Successfully processed.
                }
                catch (IOException ioEx)
                {
                    if (attempt == maxRetries)
                    {
                        Console.Error.WriteLine($"Failed to open MBOX after {maxRetries} attempts: {ioEx.Message}");
                        return;
                    }

                    Console.Error.WriteLine($"Attempt {attempt} failed due to I/O error: {ioEx.Message}. Retrying in {retryDelayMs} ms...");
                    Thread.Sleep(retryDelayMs);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                    return;
                }
            }

            if (!opened)
                Console.Error.WriteLine("Unable to open the MBOX file.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
