using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";
            string mboxPath = "output.mbox";

            // Verify input file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Ensure output directory exists
            string mboxDir = Path.GetDirectoryName(mboxPath);
            if (!string.IsNullOrEmpty(mboxDir) && !Directory.Exists(mboxDir))
            {
                Directory.CreateDirectory(mboxDir);
            }

            // Load the MSG file into a MapiMessage
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Create an in‑memory PST (Unicode format)
                using (MemoryStream pstStream = new MemoryStream())
                {
                    using (PersonalStorage pst = PersonalStorage.Create(pstStream, FileFormatVersion.Unicode))
                    {
                        // Get the standard Inbox folder
                        FolderInfo inbox = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                        // Add the message to the Inbox
                        inbox.AddMessage(msg);

                        // Convert the PST to an MBOX file
                        MailboxConverter.ConvertPersonalStorageToMbox(pst, mboxPath, null);
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
