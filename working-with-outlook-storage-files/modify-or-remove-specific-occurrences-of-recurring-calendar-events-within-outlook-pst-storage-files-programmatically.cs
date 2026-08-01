using Aspose.Email.Calendar;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailPstCalendarEdit
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                const string pstPath = "storage.pst";

                // Ensure the PST file exists; create a minimal one if missing
                if (!File.Exists(pstPath))
                {
                    try
                    {
                        // Create a new Unicode PST file
                        PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                        Console.WriteLine($"Created new PST file at '{pstPath}'.");
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
                    try
                    {
                        // Get the Calendar folder (standard IPM folder)
                        FolderInfo calendarFolder = pst.GetPredefinedFolder(StandardIpmFolder.Appointments);

                        // Define the subject of the recurring appointment to remove
                        const string targetSubject = "Team Meeting";

                        // Iterate through all messages in the Calendar folder
                        foreach (MessageInfo messageInfo in calendarFolder.EnumerateMessages())
                        {
                            // Identify appointments by subject (you could also inspect recurrence properties)
                            if (string.Equals(messageInfo.Subject, targetSubject, StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    // Convert the entry ID (byte[]) to a string representation required by DeleteItem
                                    string entryIdString = Convert.ToBase64String(messageInfo.EntryId);
                                    // Delete the entire recurring series (or a single occurrence) by entry ID
                                    pst.DeleteItem(entryIdString);
                                    Console.WriteLine($"Deleted appointment with Subject '{targetSubject}' (EntryId: {entryIdString}).");
                                }
                                catch (Exception delEx)
                                {
                                    Console.Error.WriteLine($"Failed to delete item '{messageInfo.EntryId}': {delEx.Message}");
                                }
                            }
                        }
                    }
                    catch (Exception pstEx)
                    {
                        Console.Error.WriteLine($"Error processing PST: {pstEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
