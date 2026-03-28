using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string mboxPath = "storage.mbox";

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Error: File not found – {mboxPath}");
                return;
            }

            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                foreach (Aspose.Email.Storage.Mbox.MboxMessageInfo messageInfo in mbox.EnumerateMessageInfo())
                {
                    Console.WriteLine($"Subject: {messageInfo.Subject}");
                    Console.WriteLine($"From: {messageInfo.From}");
                    Console.WriteLine($"To: {messageInfo.To}");

                    using (MailMessage eml = mbox.ExtractMessage(messageInfo.EntryId, new EmlLoadOptions()))
                    {
                        string subject = string.IsNullOrEmpty(eml.Subject) ? "message" : eml.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            subject = subject.Replace(c, '_');
                        }

                        string outputPath = $"{subject}.eml";

                        try
                        {
                            eml.Save(outputPath);
                            Console.WriteLine($"Saved: {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                        }
                    }

                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
