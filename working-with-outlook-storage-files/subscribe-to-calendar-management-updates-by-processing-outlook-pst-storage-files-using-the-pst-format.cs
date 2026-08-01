using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace PSTCalendarProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "calendar.pst";

                // Ensure PST file exists; create a placeholder if missing
                if (!File.Exists(pstPath))
                {
                    Console.WriteLine($"PST file not found. Creating placeholder at {pstPath}");
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }

                // Open PST storage
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Process root folder recursively
                    ProcessFolder(pst.RootFolder, pst);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ProcessFolder(FolderInfo folderInfo, PersonalStorage pst)
        {
            try
            {
                Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                Console.WriteLine($"Unread items: {folderInfo.ContentUnreadCount}");

                // Enumerate messages in the current folder
                foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                {
                    Console.WriteLine($"Subject: {messageInfo.Subject}");

                    // Extract the full MAPI message
                    MapiMessage mapiMessage = pst.ExtractMessage(messageInfo);

                    // Check if the message is a calendar item
                    if (mapiMessage.SupportedType == MapiItemType.Calendar)
                    {
                        // Convert to a generic MAPI item and then handle as MapiCalendar
                        var mapiItem = mapiMessage.ToMapiMessageItem();
                        if (mapiItem is MapiCalendar calendar)
                        {
                            Console.WriteLine($"  Calendar Subject: {calendar.Subject}");
                            Console.WriteLine($"  Start: {calendar.StartDate}");
                            Console.WriteLine($"  End:   {calendar.EndDate}");
                            Console.WriteLine($"  Location: {calendar.Location}");
                        }
                    }
                }

                // Recursively process subfolders
                foreach (FolderInfo subFolder in folderInfo.GetSubFolders())
                {
                    ProcessFolder(subFolder, pst);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Folder processing error: {ex.Message}");
            }
        }
    }
}
