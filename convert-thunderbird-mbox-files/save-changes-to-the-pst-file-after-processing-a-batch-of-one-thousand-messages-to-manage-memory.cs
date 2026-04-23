using System;
using System.IO;
using Aspose.Email;
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
            const string mboxPath = "input.mbox";
            const string pstPath = "output.pst";

            // Ensure MBOX file exists; create minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Open or create PST file with write access
            PersonalStorage pst;
            if (File.Exists(pstPath))
            {
                try
                {
                    pst = PersonalStorage.FromFile(pstPath, true);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to open PST file: {ex.Message}");
                    return;
                }
            }
            else
            {
                try
                {
                    pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            using (pst)
            {
                // Get or create the Inbox folder in PST
                FolderInfo inboxFolder = pst.RootFolder.GetSubFolder("Inbox");
                if (inboxFolder == null)
                {
                    inboxFolder = pst.RootFolder.AddSubFolder("Inbox");
                }

                // Open MBOX reader
                MboxStorageReader mboxReader;
                try
                {
                    mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to open MBOX file: {ex.Message}");
                    return;
                }

                using (mboxReader)
                {
                    const int batchSize = 1000;
                    int processedCount = 0;

                    foreach (MailMessage mailMessage in mboxReader.EnumerateMessages())
                    {
                        // Convert MailMessage to MapiMessage
                        MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                        // Add message to PST folder
                        inboxFolder.AddMessage(mapiMessage);
                        processedCount++;

                        // Save PST after each batch to free memory
                        if (processedCount % batchSize == 0)
                        {
                            try
                            {
                                pst.SaveAs(pstPath, FileFormat.Pst);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save PST after batch: {ex.Message}");
                                // Continue processing remaining messages
                            }
                        }
                    }

                    // Final save for any remaining messages
                    try
                    {
                        pst.SaveAs(pstPath, FileFormat.Pst);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save final PST: {ex.Message}");
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
