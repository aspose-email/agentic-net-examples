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
            // Path to the PST file
            string pstPath = "sample.pst";

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Open existing PST or create a new one (Unicode format)
            PersonalStorage pst;
            if (File.Exists(pstPath))
            {
                pst = PersonalStorage.FromFile(pstPath);
            }
            else
            {
                pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
            }

            using (pst)
            {
                // Create a simple MAPI calendar item
                MapiCalendar calendar = new MapiCalendar(
                    location: "Conference Room",
                    summary: "Project Meeting",
                    description: "Discuss project milestones",
                    startDate: new DateTime(2023, 10, 1, 10, 0, 0),
                    endDate: new DateTime(2023, 10, 1, 11, 0, 0));

                // Add the calendar item to the root folder of the PST
                string entryId = pst.RootFolder.AddMapiMessageItem(calendar);
                Console.WriteLine($"Calendar item added. EntryId: {entryId}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
