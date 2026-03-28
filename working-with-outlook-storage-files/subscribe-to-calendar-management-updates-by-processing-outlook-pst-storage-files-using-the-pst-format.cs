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

            // Ensure the PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: File not found – {pstPath}");
                return;
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Locate the Calendar folder (by display name)
                FolderInfo calendarFolder = null;
                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    if (string.Equals(folder.DisplayName, "Calendar", StringComparison.OrdinalIgnoreCase))
                    {
                        calendarFolder = folder;
                        break;
                    }
                }

                if (calendarFolder == null)
                {
                    Console.Error.WriteLine("Error: Calendar folder not found in the PST.");
                    return;
                }

                // Enumerate calendar items
                foreach (MessageInfo msgInfo in calendarFolder.EnumerateMessages())
                {
                    using (MapiMessage mapiMsg = pst.ExtractMessage(msgInfo))
                    {
                        // Process only calendar items
                        if (mapiMsg.SupportedType == MapiItemType.Calendar)
                        {
                            MapiCalendar calendar = (MapiCalendar)mapiMsg.ToMapiMessageItem();

                            Console.WriteLine($"Subject: {calendar.Subject}");
                            Console.WriteLine($"Start: {calendar.StartDate}");
                            Console.WriteLine($"End: {calendar.EndDate}");
                            Console.WriteLine($"Location: {calendar.Location}");
                            Console.WriteLine(new string('-', 40));
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
