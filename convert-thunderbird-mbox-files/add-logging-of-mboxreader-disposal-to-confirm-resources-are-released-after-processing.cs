using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

namespace MboxReaderExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                const string mboxPath = "storage.mbox";
                const string outputDir = "output";

                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                    return;
                }

                Directory.CreateDirectory(outputDir);

                MboxStorageReader mbox = null;
                try
                {
                    mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions());

                    MailMessage message;
                    while ((message = mbox.ReadNextMessage()) != null)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"To: {message.To}");

                        string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        string emlFileName = Path.Combine(outputDir, $"{safeSubject}.eml");
                        message.Save(emlFileName);
                    }
                }
                finally
                {
                    if (mbox != null)
                    {
                        mbox.Dispose();
                        Console.WriteLine("MboxStorageReader disposed.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
