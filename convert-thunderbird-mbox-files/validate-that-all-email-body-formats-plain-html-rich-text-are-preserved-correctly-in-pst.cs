using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define input MBOX and output PST paths
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Ensure the directory for PST exists
            try
            {
                string pstDir = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(pstDir) && !Directory.Exists(pstDir))
                {
                    Directory.CreateDirectory(pstDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare PST directory: {ex.Message}");
                return;
            }

            // Verify MBOX file existence; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (FileStream fs = File.Create(mboxPath))
                    {
                        // Write a minimal empty MBOX placeholder
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Convert MBOX to PST
            PersonalStorage pst;
            try
            {
                pst = MailStorageConverter.MboxToPst(mboxPath, pstPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"MBOX to PST conversion failed: {ex.Message}");
                return;
            }

            using (pst)
            {
                // Get the Inbox folder (create if it does not exist)
                FolderInfo inboxFolder;
                try
                {
                    inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to get Inbox folder: {ex.Message}");
                    return;
                }

                // Enumerate messages and verify body formats
                foreach (MessageInfo msgInfo in inboxFolder.EnumerateMessages())
                {
                    try
                    {
                        using (MapiMessage mapiMsg = pst.ExtractMessage(msgInfo))
                        {
                            Console.WriteLine($"Subject: {mapiMsg.Subject}");

                            // Plain text body
                            string plainBody = mapiMsg.Body ?? string.Empty;
                            Console.WriteLine($"Plain Body Length: {plainBody.Length}");

                            // HTML body
                            string htmlBody = mapiMsg.BodyHtml ?? string.Empty;
                            Console.WriteLine($"HTML Body Length: {htmlBody.Length}");

                            // RTF body
                            string rtfBody = mapiMsg.BodyRtf ?? string.Empty;
                            Console.WriteLine($"RTF Body Length: {rtfBody.Length}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing message ID {msgInfo.EntryId}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
