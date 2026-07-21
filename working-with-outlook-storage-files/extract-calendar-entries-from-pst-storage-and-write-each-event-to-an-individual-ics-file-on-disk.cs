using System;
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
            string pstPath = "storage.pst";
            string outputDirectory = "CalendarEvents";

            if (!File.Exists(pstPath) || new FileInfo(pstPath).Length == 0)
            {
                if (File.Exists(pstPath))
                {
                    File.Delete(pstPath);
                }
                using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                }
            }

            Directory.CreateDirectory(outputDirectory);

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                FolderInfo calendarFolder = pst.GetPredefinedFolder(StandardIpmFolder.Appointments);
                if (calendarFolder == null)
                {
                    Console.WriteLine("The PST file does not contain an appointments folder.");
                    return;
                }

                foreach (MessageInfo messageInfo in calendarFolder.EnumerateMessages())
                {
                    using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                    {
                        if (mapiMessage.SupportedType == MapiItemType.Calendar)
                        {
                            MapiCalendar calendar = (MapiCalendar)mapiMessage.ToMapiMessageItem();
                            string title = string.IsNullOrWhiteSpace(calendar.Subject) ? "Untitled" : calendar.Subject;
                            string fileName = MakeSafeFileName(title);
                            string outputPath = Path.Combine(outputDirectory, $"{fileName}.ics");

                            int duplicateIndex = 1;
                            while (File.Exists(outputPath))
                            {
                                outputPath = Path.Combine(outputDirectory, $"{fileName}_{duplicateIndex}.ics");
                                duplicateIndex++;
                            }

                            calendar.Save(outputPath, new MapiCalendarIcsSaveOptions());
                            Console.WriteLine($"Saved calendar event: {outputPath}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static string MakeSafeFileName(string value)
    {
        string safe = string.IsNullOrWhiteSpace(value) ? "Untitled" : value.Trim();
        foreach (char invalidChar in Path.GetInvalidFileNameChars())
        {
            safe = safe.Replace(invalidChar, '_');
        }
        return safe;
    }
}
