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

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                int index = 0;
                MailMessage message;
                while ((message = mbox.ReadNextMessage()) != null)
                {
                    Console.WriteLine($"Message #{index + 1}");
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.From}");
                    Console.WriteLine($"To: {string.Join(", ", message.To)}");

                    string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "No_Subject" : message.Subject;
                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        safeSubject = safeSubject.Replace(c, '_');
                    }

                    string filePath = Path.Combine(outputDir, $"{index + 1}_{safeSubject}.eml");

                    try
                    {
                        message.Save(filePath);
                        Console.WriteLine($"Saved: {filePath}");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Failed to save message '{message.Subject}': {ioEx.Message}");
                    }

                    index++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
