using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    using (PersonalStorage createdPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // No additional actions needed; just close after creation
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file for read/write operations
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Create target subfolders under the root folder
                FolderInfo invoicesFolder;
                FolderInfo othersFolder;

                try
                {
                    invoicesFolder = pst.RootFolder.AddSubFolder("Invoices");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create 'Invoices' folder: {ex.Message}");
                    return;
                }

                try
                {
                    othersFolder = pst.RootFolder.AddSubFolder("Others");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create 'Others' folder: {ex.Message}");
                    return;
                }

                // Enumerate messages in the root folder
                foreach (MessageInfo messageInfo in pst.RootFolder.EnumerateMessages())
                {
                    // Simple selection criteria: move messages with "Invoice" in the subject
                    bool isInvoice = false;
                    try
                    {
                        // Extract the message to read its subject
                        using (MapiMessage tempMessage = pst.ExtractMessage(messageInfo))
                        {
                            if (tempMessage != null && tempMessage.Subject != null &&
                                tempMessage.Subject.IndexOf("Invoice", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                isInvoice = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to read message subject: {ex.Message}");
                        continue; // Skip this message and continue processing others
                    }

                    // Move the message to the appropriate folder
                    try
                    {
                        if (isInvoice)
                        {
                            pst.MoveItem(messageInfo, invoicesFolder);
                        }
                        else
                        {
                            pst.MoveItem(messageInfo, othersFolder);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to move message '{messageInfo.Subject}': {ex.Message}");
                        // Continue processing remaining messages
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
