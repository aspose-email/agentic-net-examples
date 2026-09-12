using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the Thunderbird MBOX file
            const string mboxPath = "thunderbird.mbox";
            // Directory where converted files will be saved
            const string outputDir = "Converted";

            // Verify that the source MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {mboxPath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Create a reader for the MBOX storage
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                foreach (MboxMessageInfo mboxMessageInfo in mboxReader.EnumerateMessageInfo())
                {
                    // Extract the full MIME message from the MBOX file
                    using (MailMessage message = mboxReader.ExtractMessage(mboxMessageInfo.EntryId, new EmlLoadOptions()))
                    {
                        // Build a safe file name from the message subject
                        string subject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            subject = subject.Replace(c, '_');

                        // Save as .eml
                        string emlPath = Path.Combine(outputDir, $"{subject}.eml");
                        message.Save(emlPath);

                        // Also save as .msg using default MSG save options
                        string msgPath = Path.Combine(outputDir, $"{subject}.msg");
                        message.Save(msgPath, SaveOptions.DefaultMsg);
                    }
                }
            }

            // Optional: Convert the entire MBOX file to a PST file
            string pstPath = Path.Combine(outputDir, "Archive.pst");
            try
            {
                // This method returns a PersonalStorage instance representing the created PST
                PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxPath, pstPath);
                // Dispose the PST storage when done
                pstStorage.Dispose();
                Console.WriteLine($"MBOX successfully converted to PST: {pstPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"PST conversion failed: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
