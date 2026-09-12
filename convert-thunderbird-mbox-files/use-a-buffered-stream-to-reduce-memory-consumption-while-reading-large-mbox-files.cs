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
                Console.Error.WriteLine($"Input file not found: {mboxPath}");
                return;
            }

            const string outputDir = "ExtractedMessages";
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            using (FileStream fileStream = new FileStream(mboxPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BufferedStream bufferedStream = new BufferedStream(fileStream, 81920))
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(bufferedStream, new MboxLoadOptions()))
            {
                int messageIndex = 0;
                MailMessage message;
                while ((message = mboxReader.ReadNextMessage()) != null)
                {
                    messageIndex++;
                    Console.WriteLine($"Message {messageIndex}: Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.From}");
                    Console.WriteLine($"To: {message.To}");

                    string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                    foreach (char c in Path.GetInvalidFileNameChars())
                        safeSubject = safeSubject.Replace(c, '_');

                    string fileName = $"{safeSubject}_{messageIndex}.eml";
                    string outputPath = Path.Combine(outputDir, fileName);

                    message.Save(outputPath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
