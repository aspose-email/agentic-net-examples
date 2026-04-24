using Aspose.Email.Mapi;
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
            RunMissingSubjectTest();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }

    static void RunMissingSubjectTest()
    {
        string tempDir = Path.Combine(Path.GetTempPath(), "AsposeEmailMboxTest");
        string mboxPath = Path.Combine(tempDir, "test.mbox");
        string pstPath = Path.Combine(tempDir, "output.pst");

        // Ensure temporary directory exists
        try
        {
            if (!Directory.Exists(tempDir))
                Directory.CreateDirectory(tempDir);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create temp directory: {ex.Message}");
            return;
        }

        // Create placeholder MBOX file with a message missing Subject header
        if (!File.Exists(mboxPath))
        {
            try
            {
                using (var writer = new StreamWriter(mboxPath))
                {
                    writer.WriteLine("From - Fri Jan 01 00:00:00 2021");
                    writer.WriteLine("Date: Fri, 1 Jan 2021 00:00:00 +0000");
                    // No Subject header
                    writer.WriteLine();
                    writer.WriteLine("This is the body of the message without a subject.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write placeholder MBOX: {ex.Message}");
                return;
            }
        }

        // Convert MBOX to PST with a MailHandler that validates missing Subject
        try
        {
            MailStorageConverter.MailHandler handler = (MailMessage msg) =>
            {
                if (!string.IsNullOrEmpty(msg.Subject))
                {
                    Console.Error.WriteLine("Test failed: Message subject is not empty.");
                }
                else
                {
                    Console.WriteLine("Handler verified: Message subject is missing as expected.");
                }
            };

            PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, handler);
            // Verify the PST contains the message and its Subject is empty
            using (pst)
            {
                FolderInfo inbox = pst.RootFolder.GetSubFolder("Inbox");
                foreach (MessageInfo info in inbox.EnumerateMessages())
                {
                    using (MapiMessage mapiMsg = pst.ExtractMessage(info))
                    {
                        if (!string.IsNullOrEmpty(mapiMsg.Subject))
                        {
                            Console.Error.WriteLine("Test failed: PST message subject is not empty.");
                        }
                        else
                        {
                            Console.WriteLine("PST verification passed: Subject is missing.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Conversion or verification failed: {ex.Message}");
        }
    }
}
