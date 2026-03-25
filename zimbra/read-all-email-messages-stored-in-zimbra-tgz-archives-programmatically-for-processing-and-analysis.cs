using System.Collections.Generic;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        List<MailMessage> messages = new List<MailMessage>();

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
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                    return;
                }
            }

            using (TgzReader reader = new TgzReader(tgzPath))
            {
                try
                {
                    reader.ExportTo(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error exporting messages: {ex.Message}");
                    return;
                }

                while (true)
                {
                    bool hasMore;
                    try
                    {
                        hasMore = reader.ReadNextMessage();
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error reading next message: {ex.Message}");
                        break;
                    }

                    if (!hasMore)
                    {
                        break;
                    }

                    MailMessage message = reader.CurrentMessage;
                    if (message != null)
                    {
                        string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        string filePath = Path.Combine(outputDir, safeSubject + ".eml");
                        try
                        {
                            message.Save(filePath, SaveOptions.DefaultEml);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error saving message '{safeSubject}': {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}