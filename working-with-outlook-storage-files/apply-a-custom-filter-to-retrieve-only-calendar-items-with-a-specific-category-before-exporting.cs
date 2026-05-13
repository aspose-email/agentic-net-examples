using Aspose.Email.Calendar;
using System;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Input PST file path
            string pstPath = "sample.pst";
            // Output directory for exported calendar items
            string exportDir = "ExportedCalendars";
            // Category to filter
            string targetCategory = "ProjectX";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(exportDir))
                {
                    Directory.CreateDirectory(exportDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the Calendar (Appointments) folder
                FolderInfo calendarFolder = pst.GetPredefinedFolder(StandardIpmFolder.Appointments);

                // Enumerate messages in the calendar folder
                foreach (MessageInfo messageInfo in calendarFolder.EnumerateMessages())
                {
                    // Extract the full MAPI message
                    using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                    {
                        // Process only calendar items
                        if (mapiMessage.SupportedType == MapiItemType.Calendar)
                        {
                            // Convert to strongly typed MapiCalendar
                            MapiCalendar calendar = (MapiCalendar)mapiMessage.ToMapiMessageItem();

                            // Check if the calendar item contains the target category
                            if (calendar.Categories != null &&
                                calendar.Categories.Any(cat => cat.Equals(targetCategory, StringComparison.OrdinalIgnoreCase)))
                            {
                                // Build a safe file name
                                string safeSubject = string.IsNullOrWhiteSpace(calendar.Subject) ? "Untitled" : calendar.Subject;
                                foreach (char c in Path.GetInvalidFileNameChars())
                                {
                                    safeSubject = safeSubject.Replace(c, '_');
                                }

                                string fileName = $"{safeSubject}_{calendar.StartDate:yyyyMMddHHmm}.ics";
                                string exportPath = Path.Combine(exportDir, fileName);

                                // Save the calendar item as iCalendar file
                                try
                                {
                                    calendar.Save(exportPath);
                                    Console.WriteLine($"Exported: {exportPath}");
                                }
                                catch (Exception saveEx)
                                {
                                    Console.Error.WriteLine($"Failed to save calendar item: {saveEx.Message}");
                                }
                            }
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
