using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Verify input file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Convert MBOX to PST
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
            {
                // Access the Inbox folder (create if missing)
                FolderInfo inboxFolder;
                try
                {
                    inboxFolder = pst.RootFolder.GetSubFolder("Inbox");
                }
                catch (Exception)
                {
                    inboxFolder = pst.RootFolder.AddSubFolder("Inbox");
                }

                // Iterate through messages
                foreach (MessageInfo msgInfo in inboxFolder.EnumerateMessages())
                {
                    using (MapiMessage mapiMessage = pst.ExtractMessage(msgInfo))
                    {
                        // Set up conversion options with a watchdog timeout
                        MailConversionOptions convOptions = new MailConversionOptions
                        {
                            Timeout = 2000 // 2 seconds
                        };
                        convOptions.TimeoutReached += (sender, e) =>
                        {
                            Console.Error.WriteLine($"Timeout reached while converting message: {mapiMessage.Subject}");
                        };

                        try
                        {
                            // Perform conversion; watchdog will trigger if it exceeds Timeout
                            MailMessage mailMessage = mapiMessage.ToMailMessage(convOptions);
                            // Placeholder for further processing of mailMessage
                        }
                        catch (Aspose.Email.TimeoutException tex)
                        {
                            Console.Error.WriteLine($"Watchdog aborted conversion: {tex.Message}");
                        }
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
