using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Input PST file path (replace with actual path)
            string pstFilePath = "input.pst";

            // Output directory for .ics files
            string outputDirectory = "ExportedCalendars";

            // Verify PST file exists
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                Directory.CreateDirectory(outputDirectory);
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Get the Calendar (Appointments) folder
                FolderInfo calendarFolder = pst.GetPredefinedFolder(StandardIpmFolder.Appointments);

                // Enumerate all messages in the calendar folder
                foreach (MessageInfo messageInfo in calendarFolder.EnumerateMessages())
                {
                    // Extract the full MAPI message
                    using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                    {
                        // Process only calendar items
                        if (mapiMessage.SupportedType == MapiItemType.Calendar)
                        {
                            // Convert to MapiCalendar
                            MapiCalendar mapiCalendar = (MapiCalendar)mapiMessage.ToMapiMessageItem();

                            // Build a safe file name using subject and start date
                            string subjectPart = string.IsNullOrEmpty(mapiCalendar.Subject) ? "Untitled" : mapiCalendar.Subject;
                            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                            {
                                subjectPart = subjectPart.Replace(invalidChar, '_');
                            }

                            string timestamp = mapiCalendar.StartDate != DateTime.MinValue
                                ? mapiCalendar.StartDate.ToString("yyyyMMddHHmmss")
                                : DateTime.Now.ToString("yyyyMMddHHmmss");

                            string icsFileName = $"{subjectPart}_{timestamp}.ics";
                            string icsFilePath = Path.Combine(outputDirectory, icsFileName);

                            // Save the calendar as .ics
                            try
                            {
                                mapiCalendar.Save(icsFilePath, AppointmentSaveFormat.Ics);
                                Console.WriteLine($"Exported: {icsFilePath}");
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save calendar '{subjectPart}': {saveEx.Message}");
                            }
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
}
