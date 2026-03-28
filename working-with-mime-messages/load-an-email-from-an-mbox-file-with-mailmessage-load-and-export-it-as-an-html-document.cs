using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string mboxFilePath = "sample.mbox";
            string outputDirectory = "output";

            // Verify MBOX file exists
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxFilePath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create reader for the MBOX file
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxFilePath, new MboxLoadOptions()))
            {
                // Iterate through each message info in the storage
                foreach (MboxMessageInfo mboxMessageInfo in mboxReader.EnumerateMessageInfo())
                {
                    // Extract the full MailMessage using the entry identifier
                    using (MailMessage mailMessage = mboxReader.ExtractMessage(mboxMessageInfo.EntryId, new EmlLoadOptions()))
                    {
                        // Build a safe file name for the HTML output
                        string safeSubject = string.IsNullOrEmpty(mboxMessageInfo.Subject) ? "Untitled" : mboxMessageInfo.Subject;
                        foreach (char invalidChar in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(invalidChar, '_');
                        }

                        string htmlFilePath = Path.Combine(outputDirectory, $"{safeSubject}.html");

                        // Save the message as HTML
                        mailMessage.Save(htmlFilePath, new HtmlSaveOptions());
                        Console.WriteLine($"Saved HTML: {htmlFilePath}");
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
