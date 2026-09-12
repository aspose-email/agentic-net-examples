using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Ensure the MBOX file exists; create a minimal one with a long subject if missing.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    // Create a message with an extremely long subject.
                    string longSubject = new string('A', 500);
                    MailMessage msg = new MailMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        longSubject,
                        "This is the body of the message.");

                    // Save the message to a temporary EML file.
                    string tempEml = Path.GetTempFileName();
                    msg.Save(tempEml, SaveOptions.DefaultEml);

                    // Build a simple MBOX entry.
                    string mboxEntry = "From - " + DateTime.Now.ToString("ddd MMM dd HH:mm:ss yyyy") + "\r\n" +
                                       File.ReadAllText(tempEml) + "\r\n";

                    // Write the MBOX file.
                    File.WriteAllText(mboxPath, mboxEntry);
                    File.Delete(tempEml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for the PST file exists.
            try
            {
                string pstDir = Path.GetDirectoryName(Path.GetFullPath(pstPath));
                if (!string.IsNullOrEmpty(pstDir) && !Directory.Exists(pstDir))
                {
                    Directory.CreateDirectory(pstDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to ensure output directory: {ex.Message}");
                return;
            }

            // Convert MBOX to PST using the static helper.
            try
            {
                MailStorageConverter.MboxToPst(mboxPath, pstPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }

            // Open the resulting PST and enumerate messages to inspect subject lengths.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    ProcessFolder(pst.RootFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively enumerate messages in a PST folder and output subject information.
    private static void ProcessFolder(FolderInfo folder)
    {
        try
        {
            // List messages in the current folder.
            MessageInfoCollection messages = folder.GetContents();
            foreach (MessageInfo info in messages)
            {
                // Subject may be truncated in PST; display its length.
                Console.WriteLine($"Folder: {folder.DisplayName}, Subject length: {info.Subject?.Length ?? 0}");
            }

            // Recurse into subfolders.
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(subFolder);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing folder '{folder?.DisplayName}': {ex.Message}");
        }
    }
}
