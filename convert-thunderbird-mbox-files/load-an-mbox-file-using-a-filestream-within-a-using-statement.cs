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

            using (FileStream fileStream = new FileStream(mboxPath, FileMode.Open, FileAccess.Read))
            {
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(fileStream, new MboxLoadOptions()))
                {
                    int index = 0;
                    while (true)
                    {
                        MailMessage message = mboxReader.ReadNextMessage();
                        if (message == null)
                            break;

                        using (message)
                        {
                            string safeSubject = string.IsNullOrWhiteSpace(message.Subject)
                                ? $"Message_{index}"
                                : message.Subject;

                            foreach (char c in Path.GetInvalidFileNameChars())
                                safeSubject = safeSubject.Replace(c, '_');

                            string fileName = $"{safeSubject}_{index}.eml";
                            string outputPath = Path.Combine(outputDir, fileName);
                            message.Save(outputPath);
                            Console.WriteLine($"Saved: {outputPath}");
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
