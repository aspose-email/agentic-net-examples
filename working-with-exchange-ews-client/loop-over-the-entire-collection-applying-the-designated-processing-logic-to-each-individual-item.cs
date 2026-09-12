using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

namespace MboxProcessingSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Path to the source MBOX file.
                string mboxPath = "storage.mbox";

                // Verify that the MBOX file exists before attempting to read it.
                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                    return;
                }

                // Ensure the output directory exists.
                string outputDir = "output";
                Directory.CreateDirectory(outputDir);

                // Create the MboxStorageReader instance.
                MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions());

                // Iterate through each message info object in the MBOX storage.
                foreach (MboxMessageInfo messageInfo in mbox.EnumerateMessageInfo())
                {
                    Console.WriteLine($"Subject: {messageInfo.Subject}");
                    Console.WriteLine($"From: {messageInfo.From}");
                    Console.WriteLine($"To: {messageInfo.To}");

                    // Extract the full MIME message using the entry ID.
                    MailMessage eml = mbox.ExtractMessage(messageInfo.EntryId, new EmlLoadOptions());

                    // Prepare a safe filename for the extracted message.
                    string subject = string.IsNullOrWhiteSpace(eml.Subject) ? "No_Subject" : eml.Subject;
                    string fileName = $"{subject}.eml";
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        fileName = fileName.Replace(invalidChar, '_');
                    }

                    // Combine with the output directory.
                    string outputPath = Path.Combine(outputDir, fileName);

                    // Save the extracted message as an .eml file.
                    eml.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
