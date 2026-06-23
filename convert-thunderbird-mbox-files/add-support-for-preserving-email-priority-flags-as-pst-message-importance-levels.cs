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
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Ensure the MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(mboxPath))
                    {
                        writer.WriteLine("From - Mon Jan 01 00:00:00 2020");
                        writer.WriteLine("Subject: Test");
                        writer.WriteLine("From: sender@example.com");
                        writer.WriteLine("To: recipient@example.com");
                        writer.WriteLine();
                        writer.WriteLine("This is a test email.");
                        writer.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Convert MBOX to PST
            try
            {
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
                {
                    // PST created; disposal handled by using
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"MBOX to PST conversion failed: {ex.Message}");
                return;
            }

            // Open PST, add a high‑priority message, and display importance levels
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                    // Create a MailMessage with high priority
                    using (MailMessage highPriorityMessage = new MailMessage())
                    {
                        highPriorityMessage.From = "alice@example.com";
                        highPriorityMessage.To.Add("bob@example.com");
                        highPriorityMessage.Subject = "High Priority Email";
                        highPriorityMessage.Body = "Please attend the meeting ASAP.";
                        highPriorityMessage.Priority = MailPriority.High;

                        // Convert to MapiMessage (preserves priority as importance)
                        using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(highPriorityMessage))
                        {
                            inboxFolder.AddMessage(mapiMessage);
                        }
                    }

                    // Enumerate messages and output their importance
                    foreach (MessageInfo messageInfo in inboxFolder.EnumerateMessages())
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}, Importance: {messageInfo.Importance}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"PST processing failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
