using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths for the sample MBOX and the resulting PST
            string mboxPath = "sample.mbox";
            string pstPath = "output.pst";

            // Ensure the MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (FileStream mboxStream = new FileStream(mboxPath, FileMode.Create, FileAccess.Write))
                    using (StreamWriter writer = new StreamWriter(mboxStream))
                    {
                        // Minimal MBOX format: a "From " line followed by headers and body
                        writer.WriteLine("From - Mon Jan 01 00:00:00 2020");
                        writer.WriteLine("Subject: Test Message");
                        writer.WriteLine("From: sender@example.com");
                        writer.WriteLine("To: recipient@example.com");
                        writer.WriteLine();
                        writer.WriteLine("This is a test email body.");
                        writer.WriteLine(); // End of message
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output PST file does not already exist; delete if present
            if (File.Exists(pstPath))
            {
                try
                {
                    File.Delete(pstPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to delete existing PST file: {ex.Message}");
                    return;
                }
            }

            // Perform the conversion from MBOX to PST
            try
            {
                // The static method returns a PersonalStorage instance representing the created PST
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
                {
                    // Verify that the PST was created and contains at least one message
                    FolderInfo rootFolder = pst.RootFolder;
                    bool hasMessages = false;

                    foreach (FolderInfo subFolder in rootFolder.GetSubFolders())
                    {
                        foreach (MessageInfo messageInfo in subFolder.EnumerateMessages())
                        {
                            hasMessages = true;
                            Console.WriteLine($"Message subject: {messageInfo.Subject}");
                            break; // One message is enough for verification
                        }

                        if (hasMessages) break;
                    }

                    if (!hasMessages)
                    {
                        Console.Error.WriteLine("Conversion succeeded but no messages were found in the PST.");
                    }
                    else
                    {
                        Console.WriteLine("MBOX to PST conversion verified successfully.");
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
