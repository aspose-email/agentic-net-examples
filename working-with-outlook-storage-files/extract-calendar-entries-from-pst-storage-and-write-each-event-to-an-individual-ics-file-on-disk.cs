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
            string outputDirectory = "Calendars";

            // Verify PST file existence
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: PST file not found – {pstPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {outputDirectory}. {dirEx.Message}");
                    return;
                }
            }

            // Open PST storage
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the calendar (Appointments) folder
                FolderInfo calendarFolder = pst.GetPredefinedFolder(StandardIpmFolder.Appointments);

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
                                // Convert to MapiCalendar
                                using (MapiCalendar mapiCalendar = (MapiCalendar)mapiMessage.ToMapiMessageItem())
                                {
                                    // Build a safe file name using subject and entry id
                                    string subject = string.IsNullOrEmpty(mapiCalendar.Subject) ? "Untitled" : mapiCalendar.Subject;
                                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                                    {
                                        subject = subject.Replace(invalidChar, '_');
                                    }

                                    string icsFileName = $"{subject}_{messageInfo.EntryId}.ics";
                                    string icsFilePath = Path.Combine(outputDirectory, icsFileName);

                                    // Save the calendar as an .ics file
                                    mapiCalendar.Save(icsFilePath);
                                    Console.WriteLine($"Saved calendar event to: {icsFilePath}");
                                }
                            }
                        }
                    }
                    catch (Exception msgEx)
                    {
                        Console.Error.WriteLine($"Error processing message {messageInfo.EntryId}: {msgEx.Message}");
                        // Continue with next message
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
