using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;

namespace MboxMultiThreadedConversion
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string mboxPath = "input.mbox";
                string outputDir = "output";

                if (!File.Exists(mboxPath))
                {
                    File.WriteAllText(mboxPath, string.Empty);
                }

                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Read all messages sequentially using ReadNextMessage()
                var messages = new List<(MailMessage Message, int Index)>();
                using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    int idx = 0;
                    while (true)
                    {
                        MailMessage msg = reader.ReadNextMessage();
                        if (msg == null)
                            break;
                        messages.Add((msg, idx));
                        idx++;
                    }
                }

                // Process each message in parallel
                Parallel.ForEach(messages, item =>
                {
                    try
                    {
                        MailMessage eml = item.Message;
                        int index = item.Index;

                        string subject = string.IsNullOrEmpty(eml.Subject) ? "NoSubject" : eml.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            subject = subject.Replace(c, '_');
                        }

                        string outPath = Path.Combine(outputDir, $"{subject}_{index}.eml");
                        eml.Save(outPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to process message {item.Index}: {ex.Message}");
                    }
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
