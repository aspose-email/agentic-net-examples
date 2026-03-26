using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for Thunderbird MBOX file and the resulting PST file
            string mboxFilePath = "input.mbox";
            string pstFilePath = "output.pst";

            // Verify that the source MBOX file exists
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxFilePath}");
                return;
            }

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Convert the MBOX storage to PST
            try
            {
                using (PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath))
                {
                    // Enumerate folders and messages in the created PST (optional verification)
                    FolderInfo rootFolder = pstStorage.RootFolder;
                    foreach (FolderInfo subFolder in rootFolder.GetSubFolders())
                    {
                        Console.WriteLine($"Folder: {subFolder.DisplayName}");
                        foreach (MessageInfo messageInfo in subFolder.EnumerateMessages())
                        {
                            Console.WriteLine($"Message EntryId: {messageInfo.EntryId}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"MBOX to PST conversion failed: {ex.Message}");
                return;
            }

            // Example: Convert a single EML file (exported from Thunderbird) to MSG format
            string emlFilePath = "sample.eml";
            string msgFilePath = "sample.msg";

            if (File.Exists(emlFilePath))
            {
                try
                {
                    using (MailMessage mailMessage = MailMessage.Load(emlFilePath))
                    {
                        using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                        {
                            mapiMessage.Save(msgFilePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EML to MSG conversion failed: {ex.Message}");
                }
            }
            else
            {
                Console.Error.WriteLine($"EML file not found: {emlFilePath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}