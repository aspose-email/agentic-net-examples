using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

namespace ArchivingDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                const string mboxPath = "storage.mbox";

                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"Input file not found: {mboxPath}");
                    return;
                }

                // Ensure the output directory exists
                const string outputDir = "output";
                Directory.CreateDirectory(outputDir);

                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    foreach (MboxMessageInfo messageInfo in mboxReader.EnumerateMessageInfo())
                    {
                        try
                        {
                            MailMessage emlMessage = mboxReader.ExtractMessage(messageInfo.EntryId, new EmlLoadOptions());

                            string safeSubject = string.IsNullOrWhiteSpace(emlMessage.Subject) ? "Untitled" : emlMessage.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                            {
                                safeSubject = safeSubject.Replace(c, '_');
                            }

                            string outputPath = Path.Combine(outputDir, $"{safeSubject}.eml");

                            emlMessage.Save(outputPath);
                            Console.WriteLine($"Saved: {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to process message ID {messageInfo.EntryId}: {ex.Message}");
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
}
