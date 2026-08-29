using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailPstBatchProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            // Input PST file path
            string pstPath = "input.pst";

            // Output directory for extracted .msg files
            string outputDir = "ExtractedMessages";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Verify the PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            try
            {
                // Open the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    int processedCount = 0;

                    // Process each top‑level folder
                    foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                    {
                        ProcessFolder(pst, folder, outputDir, ref processedCount);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST: {ex.Message}");
            }
        }

        static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputDir, ref int processedCount)
        {
            // Enumerate messages in the current folder
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                // Extract the message as a MapiMessage
                MapiMessage mapiMessage = pst.ExtractMessage(messageInfo);

                // Convert to MailMessage using proper MailConversionOptions
                MailConversionOptions conversionOptions = new MailConversionOptions();
                MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions);

                // Build a safe file name
                string subject = string.IsNullOrWhiteSpace(mailMessage.Subject) ? "NoSubject" : mailMessage.Subject;
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    subject = subject.Replace(c, '_');
                }
                string fileName = $"{subject}_{messageInfo.EntryId}.msg";
                string filePath = Path.Combine(outputDir, fileName);

                // Save the message, guarding against I/O errors
                try
                {
                    mailMessage.Save(filePath);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save message '{subject}': {saveEx.Message}");
                }

                processedCount++;

                // After each batch of 1000 messages, persist PST changes
                if (processedCount % 1000 == 0)
                {
                    // PersonalStorage persists changes on Dispose; if a Save method exists, it can be called here.
                    // Uncomment the following line if the API provides a Save method:
                    // pst.Save();
                }
            }

            // Recursively process subfolders
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(pst, subFolder, outputDir, ref processedCount);
            }
        }
    }
}
