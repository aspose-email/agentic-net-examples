using Aspose.Email.Calendar;
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
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the Calendar (Appointments) folder
                FolderInfo calendarFolder;
                try
                {
                    calendarFolder = pst.GetPredefinedFolder(StandardIpmFolder.Appointments);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to get Calendar folder: {ex.Message}");
                    return;
                }

                // Create a new MapiCalendar item
                MapiCalendar calendar = new MapiCalendar();
                if (string.IsNullOrEmpty(calendar.Body))
                {
                    calendar.Body = "Calendar item body";
                }

                calendar.Subject = "Team Meeting";
                calendar.StartDate = DateTime.Now.AddDays(1).AddHours(10);
                calendar.EndDate = calendar.StartDate.AddHours(1);
                calendar.Location = "Conference Room";

                // Add the calendar item to the Calendar folder
                try
                {
                    string entryId = calendarFolder.AddMapiMessageItem(calendar);
                    Console.WriteLine($"Calendar item added with EntryId: {entryId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add calendar item: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
