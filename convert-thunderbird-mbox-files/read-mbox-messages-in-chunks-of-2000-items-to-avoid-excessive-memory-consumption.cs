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
            const int chunkSize = 2000;
            const string outputDir = "ExtractedMessages";

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory '{outputDir}': {dirEx.Message}");
                return;
            }

            using (var mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                int processed = 0;
                while (true)
                {
                    MailMessage message = mboxReader.ReadNextMessage();
                    if (message == null)
                        break;

                    processed++;

                    Console.WriteLine($"Processing message {processed}: Subject=\"{message.Subject}\" From=\"{message.From}\" To=\"{message.To}\"");

                    try
                    {
                        string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            safeSubject = safeSubject.Replace(c, '_');

                        // Ensure unique file name in case of duplicates
                        string fileName = $"{processed:D6}_{safeSubject}.eml";
                        string emlPath = Path.Combine(outputDir, fileName);
                        message.Save(emlPath);
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save message #{processed}: {saveEx.Message}");
                    }

                    if (processed % chunkSize == 0)
                    {
                        Console.WriteLine($"--- Processed {processed} messages ---");
                    }
                }

                Console.WriteLine($"Finished processing. Total messages extracted: {processed}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
