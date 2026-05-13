using Aspose.Email.Calendar;
using System;
using System.Diagnostics;
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
            string pstPath = "bulk_calendar.pst";

            // Ensure the PST file exists; create a minimal Unicode PST if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file for read/write
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Ensure the Appointments folder exists
                FolderInfo calendarFolder;
                try
                {
                    calendarFolder = pst.GetPredefinedFolder(StandardIpmFolder.Appointments);
                    if (calendarFolder == null)
                    {
                        pst.CreatePredefinedFolder("Appointments", StandardIpmFolder.Appointments);
                        calendarFolder = pst.GetPredefinedFolder(StandardIpmFolder.Appointments);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to access or create Appointments folder: {ex.Message}");
                    return;
                }

                // Measure bulk addition performance
                Stopwatch sw = Stopwatch.StartNew();

                for (int i = 0; i < 1000; i++)
                {
                    // Create a simple calendar item
                    MapiCalendar calendar = new MapiCalendar
                    {
                        Subject = $"Test Event {i + 1}",
                        StartDate = DateTime.Now.AddDays(i),
                        EndDate = DateTime.Now.AddDays(i).AddHours(1)
                    };

                    // Add the calendar item to the folder
                    try
                    {
                        calendarFolder.AddMapiMessageItem(calendar);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add calendar item {i + 1}: {ex.Message}");
                        // Continue adding remaining items
                    }
                }

                sw.Stop();
                Console.WriteLine($"Added 1000 calendar items in {sw.Elapsed.TotalSeconds:F2} seconds.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
