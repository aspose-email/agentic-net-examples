using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // ---------- EML to MSG conversion ----------
            string emlInputPath = "TestEml.eml";
            string msgOutputPath = "output.msg";

            if (!File.Exists(emlInputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlInputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input EML file not found: {emlInputPath}");
                return;
            }

            // Ensure the output directory exists
            string msgOutputDir = Path.GetDirectoryName(msgOutputPath);
            if (!string.IsNullOrEmpty(msgOutputDir) && !Directory.Exists(msgOutputDir))
            {
                Directory.CreateDirectory(msgOutputDir);
            }

            EmlLoadOptions emlLoadOptions = new EmlLoadOptions
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            using (MailMessage message = MailMessage.Load(emlInputPath, emlLoadOptions))
            {
                // Save as MSG using default options
                message.Save(msgOutputPath, SaveOptions.DefaultMsg);
            }

            // ---------- MBOX to PST conversion ----------
            string mboxInputPath = "input.mbox";
            string pstOutputPath = "output.pst";

            if (!File.Exists(mboxInputPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxInputPath}");
                return;
            }

            // Ensure the output directory exists
            string pstOutputDir = Path.GetDirectoryName(pstOutputPath);
            if (!string.IsNullOrEmpty(pstOutputDir) && !Directory.Exists(pstOutputDir))
            {
                Directory.CreateDirectory(pstOutputDir);
            }

            // Convert MBOX to PST; the returned PersonalStorage should be disposed
            using (PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxInputPath, pstOutputPath))
            {
                // Conversion completed; PST file is written to pstOutputPath
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
