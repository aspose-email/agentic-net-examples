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
            string ostPath = "archive.ost";
            string outputDirectory = "ExportedCalendars";

            // Verify OST file exists
            if (!File.Exists(ostPath))
            {
                Console.Error.WriteLine($"OST file not found: {ostPath}");
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

            // Open the OST/PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(ostPath))
            {
                // Get the calendar (appointments) folder
                FolderInfo calendarFolder;
                try
                {
                    calendarFolder = pst.GetPredefinedFolder(StandardIpmFolder.Appointments);
                }
                catch (Exception folderEx)
                {
                    Console.Error.WriteLine($"Unable to locate calendar folder: {folderEx.Message}");
                    return;
                }

                // Enumerate all messages in the calendar folder
                foreach (MessageInfo messageInfo in calendarFolder.EnumerateMessages())
                {
                    try
                    {
                        // Extract the full MAPI message
                        using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                        {
                            // Process only calendar items
                            if (mapiMessage.SupportedType == MapiItemType.Calendar)
                            {
                                // Convert to a strongly typed MapiCalendar
                                MapiCalendar calendar = (MapiCalendar)mapiMessage.ToMapiMessageItem();

                                // Build a safe file name
                                string subject = string.IsNullOrEmpty(calendar.Subject) ? "Untitled" : calendar.Subject;
                                foreach (char invalidChar in Path.GetInvalidFileNameChars())
                                {
                                    subject = subject.Replace(invalidChar, '_');
                                }
                                string icsFilePath = Path.Combine(outputDirectory, $"{subject}_{messageInfo.EntryIdString}.ics");

                                // Save as iCalendar (.ics)
                                calendar.Save(icsFilePath, AppointmentSaveFormat.Ics);
                                Console.WriteLine($"Exported calendar to: {icsFilePath}");
                            }
                        }
                    }
                    catch (Exception msgEx)
                    {
                        Console.Error.WriteLine($"Error processing message ID {messageInfo.EntryIdString}: {msgEx.Message}");
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
