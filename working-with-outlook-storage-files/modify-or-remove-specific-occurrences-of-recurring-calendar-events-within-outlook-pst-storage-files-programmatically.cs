using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the Calendar (Appointments) folder
                FolderInfo calendarFolder = pst.GetPredefinedFolder(StandardIpmFolder.Appointments);
                if (calendarFolder == null)
                {
                    Console.Error.WriteLine("Error: Calendar folder not found.");
                    return;
                }

                // Define the subject of recurring events to remove
                string targetSubject = "Team Meeting";

                // Enumerate all messages in the Calendar folder
                foreach (MessageInfo messageInfo in calendarFolder.EnumerateMessages())
                {
                    // Extract the full MAPI message
                    using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                    {
                        // Ensure the message is a calendar item
                        if (mapiMessage.SupportedType != MapiItemType.Calendar)
                            continue;

                        // Convert to MapiCalendar for easier access
                        MapiCalendar calendar = (MapiCalendar)mapiMessage.ToMapiMessageItem();

                        // Check if the subject matches the target
                        if (string.Equals(calendar.Subject, targetSubject, StringComparison.OrdinalIgnoreCase))
                        {
                            // Delete the entire recurring series (message) from the folder
                            calendarFolder.DeleteChildItem(messageInfo.EntryId);
                            Console.WriteLine($"Deleted recurring event: {calendar.Subject}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
