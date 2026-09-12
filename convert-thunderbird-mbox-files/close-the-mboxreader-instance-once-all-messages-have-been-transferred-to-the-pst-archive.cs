using Aspose.Email.Mapi;
using System;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailMboxToPst
{
    class Program
    {
        static void Main()
        {
            // Placeholder file paths – replace with real paths before running
            string mboxFilePath = "path/to/input.mbox";
            string pstFilePath = "path/to/output.pst";

            // Guard: skip processing when placeholders are still present
            if (mboxFilePath.Contains("path/to") || pstFilePath.Contains("path/to"))
            {
                Console.WriteLine("Placeholder file paths detected. Skipping MBOX to PST conversion.");
                return;
            }

            try
            {
                // Create PST file (Unicode format)
                using (PersonalStorage pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                {
                    // Create a folder in PST to store the messages
                    FolderInfo inboxFolder = pst.RootFolder.AddSubFolder("Inbox");

                    // Open MBOX reader with load options
                    using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxFilePath, new MboxLoadOptions()))
                    {
                        while (true)
                        {
                            // Read next message; returns null when no more messages are available
                            MailMessage message = mboxReader.ReadNextMessage();
                            if (message == null)
                                break;

                            // Add the message to the PST folder
                            inboxFolder.AddMessage(MapiMessage.FromMailMessage(message));
                        }

                        // MboxStorageReader will be closed automatically by the using statement
                    }

                    // PST will be saved and closed automatically by the using statement
                }

                Console.WriteLine("MBOX file has been successfully converted to PST.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred during conversion: {ex.Message}");
            }
        }
    }
}
