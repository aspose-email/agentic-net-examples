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
            string outputDir = "CalendarEvents";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                ProcessFolder(pst, pst.RootFolder, outputDir);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputDir)
    {
        // Enumerate messages in the current folder
        foreach (MessageInfo msgInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage msg = pst.ExtractMessage(msgInfo))
                {
                    // Check if the message is a calendar item
                    if (msg.SupportedType == MapiItemType.Calendar)
                    {
                        MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                        // Prepare a safe filename based on the subject
                        string safeSubject = string.IsNullOrWhiteSpace(calendar.Subject) ? "Untitled" : calendar.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        string icsPath = Path.Combine(outputDir, $"{safeSubject}_{Guid.NewGuid()}.ics");

                        // Save the calendar entry as an .ics file
                        calendar.Save(icsPath, new MapiCalendarIcsSaveOptions());

                        Console.WriteLine($"Saved calendar event to {icsPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to process a message: {ex.Message}");
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, outputDir);
        }
    }
}
