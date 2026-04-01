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
            // Define PST file path
            string pstPath = "SampleCalendar.pst";

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Open existing PST or create a new one if it does not exist
            using (PersonalStorage pst = File.Exists(pstPath)
                ? PersonalStorage.FromFile(pstPath)
                : PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
            {
                // Get the root folder of the PST
                FolderInfo rootFolder = pst.RootFolder;

                // Name of the calendar subfolder
                const string calendarFolderName = "My Calendar";

                // Try to get the calendar subfolder; create it if it does not exist
                FolderInfo calendarFolder;
                try
                {
                    calendarFolder = rootFolder.GetSubFolder(calendarFolderName);
                }
                catch
                {
                    calendarFolder = rootFolder.AddSubFolder(calendarFolderName);
                }

                // Create a MAPI calendar item
                MapiCalendar mapiCalendar = new MapiCalendar(
                    location: "Conference Room",
                    summary: "Team Meeting",
                    description: "Discuss project status and next steps.",
                    startDate: DateTime.Now.AddHours(1),
                    endDate: DateTime.Now.AddHours(2));

                // Add the calendar item to the folder
                calendarFolder.AddMapiMessageItem(mapiCalendar);

                Console.WriteLine("Calendar item added to PST successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
