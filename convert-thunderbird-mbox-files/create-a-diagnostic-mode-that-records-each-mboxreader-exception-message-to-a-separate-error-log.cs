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
            // Ensure the MBOX file exists; create an empty placeholder if missing.
            string mboxPath = "storage.mbox";
            if (!File.Exists(mboxPath))
            {
                File.WriteAllText(mboxPath, string.Empty);
            }

            // Prepare error log directory.
            string errorLogDir = "ErrorLogs";
            Directory.CreateDirectory(errorLogDir);

            // Create the MboxStorageReader instance.
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                int messageIndex = 0;
                while (true)
                {
                    MailMessage message = null;
                    try
                    {
                        // Read the next message sequentially.
                        message = reader.ReadNextMessage();
                        if (message == null)
                            break; // No more messages.

                        // Sanitize subject for file name.
                        string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        string emlPath = $"{safeSubject}_{messageIndex}.eml";

                        // Save the extracted message.
                        message.Save(emlPath);
                    }
                    catch (Exception ex)
                    {
                        // Record exception message to a separate error log file.
                        string logFileName = $"error_{messageIndex}.log";
                        string logPath = Path.Combine(errorLogDir, logFileName);
                        try
                        {
                            File.WriteAllText(logPath, ex.Message);
                        }
                        catch
                        {
                            // Suppress any logging failures.
                        }
                    }
                    finally
                    {
                        message?.Dispose();
                        messageIndex++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
