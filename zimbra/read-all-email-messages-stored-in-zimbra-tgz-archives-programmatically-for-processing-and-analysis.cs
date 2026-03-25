using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string tgzPath = "archive.tgz";
            string outputDir = "ExtractedMessages";

            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (TgzReader tgzReader = new TgzReader(tgzPath))
            {
                while (tgzReader.ReadNextMessage())
                {
                    MailMessage currentMessage = tgzReader.CurrentMessage;
                    if (currentMessage == null)
                    {
                        continue;
                    }

                    using (MailMessage message = currentMessage)
                    {
                        string subject = message.Subject ?? "NoSubject";
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            subject = subject.Replace(c, '_');
                        }

                        string filePath = Path.Combine(outputDir, subject + ".eml");
                        try
                        {
                            message.Save(filePath, SaveOptions.DefaultEml);
                            Console.WriteLine($"Saved message: {filePath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message '{subject}': {ex.Message}");
                        }
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