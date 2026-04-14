using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the PST file (replace with actual path as needed)
            string pstPath = "audit.pst";

            // Ensure the PST file exists; if not, create an empty one
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new empty PST file with Unicode format
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file safely
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Get the default Inbox folder (or any folder you need)
                    FolderInfo inbox = pst.RootFolder.GetSubFolder("Inbox");

                    // Enumerate all messages in the folder
                    foreach (MessageInfo msgInfo in inbox.EnumerateMessages())
                    {
                        // Extract the full message as a MapiMessage
                        using (MapiMessage message = pst.ExtractMessage(msgInfo))
                        {
                            // Retrieve follow‑up options, if any
                            FollowUpOptions options = FollowUpManager.GetOptions(message);
                            if (options != null)
                            {
                                // Log the creation (start) timestamp of the follow‑up flag
                                Console.WriteLine($"Subject: {message.Subject}");
                                Console.WriteLine($"Flag Request: {options.FlagRequest}");
                                Console.WriteLine($"Flag Start Date (creation timestamp): {options.StartDate}");
                                Console.WriteLine($"Flag Due Date: {options.DueDate}");
                                Console.WriteLine($"Reminder Time: {options.ReminderTime}");
                                Console.WriteLine($"Is Completed: {options.IsCompleted}");
                                Console.WriteLine(new string('-', 40));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
