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
                const string mboxFilePath = "storage.mbox";
                const string outputDir = "output";

                if (!File.Exists(mboxFilePath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxFilePath}");
                    return;
                }

                // Ensure the output directory exists.
                Directory.CreateDirectory(outputDir);

                using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxFilePath, new MboxLoadOptions()))
                {
                    int messageIndex = 0;
                    while (true)
                    {
                        MailMessage eml = mbox.ReadNextMessage();
                        if (eml == null)
                            break;

                        try
                        {
                            string safeSubject = string.IsNullOrWhiteSpace(eml.Subject) ? "NoSubject" : eml.Subject;
                            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                            {
                                safeSubject = safeSubject.Replace(invalidChar, '_');
                            }

                            string fileName = $"{messageIndex:D5}_{safeSubject}.eml";
                            string emlFilePath = Path.Combine(outputDir, fileName);

                            eml.Save(emlFilePath);
                            Console.WriteLine($"Saved: {emlFilePath}");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save message #{messageIndex}: {saveEx.Message}");
                        }
                        finally
                        {
                            eml.Dispose();
                        }

                        messageIndex++;
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
