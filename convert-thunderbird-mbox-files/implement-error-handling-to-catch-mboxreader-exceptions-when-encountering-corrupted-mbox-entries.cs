using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

namespace MboxReaderExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string mboxPath = "storage.mbox";

                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                    return;
                }

                string outputDir = "output";
                Directory.CreateDirectory(outputDir);

                using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    int messageIndex = 0;
                    while (true)
                    {
                        MailMessage message = null;
                        try
                        {
                            message = reader.ReadNextMessage();
                            if (message == null)
                                break;
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error reading message #{messageIndex}: {ex.Message}");
                            continue;
                        }

                        using (message)
                        {
                            string subject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                                subject = subject.Replace(c, '_');

                            string safeFileName = $"{subject}_{messageIndex}.eml";
                            string fullPath = Path.Combine(outputDir, safeFileName);

                            try
                            {
                                message.Save(fullPath);
                                Console.WriteLine($"Saved: {fullPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message #{messageIndex}: {ex.Message}");
                            }
                        }

                        messageIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
