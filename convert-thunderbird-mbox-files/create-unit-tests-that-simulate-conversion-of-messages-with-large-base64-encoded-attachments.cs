using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "test.mbox";
            string pstPath = "output.pst";

            // Ensure the directory for the files exists
            try
            {
                string directory = Path.GetDirectoryName(Path.GetFullPath(mboxPath));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to ensure directory exists: {ex.Message}");
                return;
            }

            // Create a placeholder MBOX file with a large base64 attachment if it does not exist
            if (!File.Exists(mboxPath))
            {
                try
                {
                    // Generate large random data (5 MB)
                    byte[] largeData = new byte[5 * 1024 * 1024];
                    new Random().NextBytes(largeData);

                    using (FileStream mboxFileStream = new FileStream(mboxPath, FileMode.Create, FileAccess.Write))
                    using (MboxrdStorageWriter mboxWriter = new MboxrdStorageWriter(mboxFileStream, new MboxSaveOptions()))
                    {
                        using (MemoryStream attachmentStream = new MemoryStream(largeData))
                        {
                            Attachment attachment = new Attachment(attachmentStream, "large.bin", "application/octet-stream");
                            MailMessage message = new MailMessage("sender@example.com", "receiver@example.com", "Test Message", "This is a test message with a large attachment.");
                            message.Attachments.Add(attachment);
                            mboxWriter.WriteMessage(message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX: {ex.Message}");
                    return;
                }
            }

            // Convert the MBOX to PST using the correct overload (string, string)
            try
            {
                PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxPath, pstPath);
                // Verify conversion by enumerating messages in the PST
                using (PersonalStorage pst = pstStorage)
                {
                    FolderInfo rootFolder = pst.RootFolder;
                    foreach (MessageInfo messageInfo in rootFolder.EnumerateMessages())
                    {
                        Console.WriteLine($"Message EntryId: {messageInfo.EntryId}");
                        using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                        {
                            Console.WriteLine($"Subject: {mapiMessage.Subject}");
                            Console.WriteLine($"Attachment count: {mapiMessage.Attachments.Count}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
