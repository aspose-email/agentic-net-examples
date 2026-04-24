using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths for PST and a test MSG file.
            string pstPath = "sample.pst";
            string testMsgPath = "test.msg";

            // Ensure the PST file exists; create a minimal one if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file.
                    using (PersonalStorage createdPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create a standard Inbox folder.
                        createdPst.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file for reading/writing.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Check if the Inbox has any messages; if not, add a simple placeholder message.
                FolderInfo inbox = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                bool hasMessages = false;
                foreach (MessageInfo _ in inbox.EnumerateMessages())
                {
                    hasMessages = true;
                    break;
                }

                if (!hasMessages)
                {
                    // Create a simple MailMessage.
                    using (MailMessage simpleMail = new MailMessage("from@example.com", "to@example.com", "Test Subject", "Test body"))
                    {
                        // Convert to MapiMessage.
                        using (MapiMessage mapiMsg = MapiMessage.FromMailMessage(simpleMail))
                        {
                            // Add the message to the Inbox.
                            string entryId = inbox.AddMessage(mapiMsg);

                            // Extract the message back to verify it can be saved as MSG (Outlook format).
                            using (MapiMessage extracted = pst.ExtractMessage(entryId))
                            {
                                try
                                {
                                    extracted.Save(testMsgPath);
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Failed to save test MSG file: {saveEx.Message}");
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    // If messages already exist, optionally extract the first one to verify Outlook compatibility.
                    foreach (MessageInfo info in inbox.EnumerateMessages())
                    {
                        using (MapiMessage extracted = pst.ExtractMessage(info))
                        {
                            try
                            {
                                extracted.Save(testMsgPath);
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save test MSG file: {saveEx.Message}");
                                return;
                            }
                        }
                        break; // Only need to test one message.
                    }
                }
            }

            Console.WriteLine("PST validation completed successfully. Test MSG file created at: " + Path.GetFullPath(testMsgPath));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
