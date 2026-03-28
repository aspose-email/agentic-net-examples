using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace AsposeEmailPstExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "sample.pst";

                // Ensure the PST file exists; create a minimal one if it does not.
                if (!File.Exists(pstPath))
                {
                    try
                    {
                        PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                        Console.WriteLine($"Created new PST file at \"{pstPath}\".");
                    }
                    catch (Exception createEx)
                    {
                        Console.Error.WriteLine($"Error creating PST file: {createEx.Message}");
                        return;
                    }
                }

                try
                {
                    // Open the existing PST file.
                    using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                    {
                        if (!pst.CanWrite)
                        {
                            Console.Error.WriteLine("The PST file is read‑only and cannot be modified.");
                            return;
                        }

                        // Get the Inbox predefined folder.
                        FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                        // Create a simple MAPI message.
                        using (MapiMessage message = new MapiMessage(
                            "sender@example.com",
                            "recipient@example.com",
                            "Sample Subject",
                            "This is a sample body of the message."))
                        {
                            // Add the message to the Inbox folder.
                            string entryId = inboxFolder.AddMessage(message);
                            Console.WriteLine($"Message added successfully. EntryId: {entryId}");
                        }
                    }
                }
                catch (Exception pstEx)
                {
                    Console.Error.WriteLine($"Error processing PST file: {pstEx.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
