using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            // Input directory containing MSG files
            string msgDirectory = "MsgFiles";
            // Temporary PST file to collect messages
            string pstPath = "TempOutput.pst";
            // Final MBOX file
            string mboxPath = "Output.mbox";

            // Verify input directory exists
            if (!Directory.Exists(msgDirectory))
            {
                Console.Error.WriteLine($"Error: Directory not found – {msgDirectory}");
                return;
            }

            // Ensure any existing PST file is removed
            if (File.Exists(pstPath))
            {
                try { File.Delete(pstPath); } catch { /* ignore */ }
            }

            // Advanced conversion options for MSG → MailMessage (preserve metadata)
            MailConversionOptions conversionOptions = new MailConversionOptions
            {
                PreserveEmbeddedMessageFormat = true,
                PreserveRtfContent = true
            };

            // Create PST storage and add each MSG as a MapiMessage
            using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
            {
                FolderInfo rootFolder = pst.RootFolder;

                foreach (string msgFile in Directory.GetFiles(msgDirectory, "*.msg"))
                {
                    // Load MSG as MailMessage
                    MailMessage mailMessage = MailMessage.Load(msgFile);

                    // Convert to MapiMessage (advanced options applied)
                    MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                    // Add the message to the PST root folder
                    rootFolder.AddMessage(mapiMessage);
                }

                // Convert the populated PST to MBOX format
                MailboxConverter.ConvertPersonalStorageToMbox(pst, mboxPath, null);
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
