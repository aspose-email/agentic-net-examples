using Aspose.Email.Calendar;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

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

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Create predefined folders
                try
                {
                    FolderInfo calendarFolder = pst.CreatePredefinedFolder("My Calendar", StandardIpmFolder.Appointments);
                    FolderInfo tasksFolder = pst.CreatePredefinedFolder("My Tasks", StandardIpmFolder.Tasks);
                    FolderInfo journalFolder = pst.CreatePredefinedFolder("My Journal", StandardIpmFolder.Journal);
                    FolderInfo notesFolder = pst.CreatePredefinedFolder("My Notes", StandardIpmFolder.Notes);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create predefined folders: {ex.Message}");
                }

                // Verify folder creation
                try
                {
                    FolderInfo calendar = pst.GetPredefinedFolder(StandardIpmFolder.Appointments);
                    FolderInfo tasks = pst.GetPredefinedFolder(StandardIpmFolder.Tasks);
                    FolderInfo journal = pst.GetPredefinedFolder(StandardIpmFolder.Journal);
                    FolderInfo notes = pst.GetPredefinedFolder(StandardIpmFolder.Notes);

                    Console.WriteLine($"Calendar folder: {calendar.DisplayName}");
                    Console.WriteLine($"Tasks folder: {tasks.DisplayName}");
                    Console.WriteLine($"Journal folder: {journal.DisplayName}");
                    Console.WriteLine($"Notes folder: {notes.DisplayName}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to verify folders: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
