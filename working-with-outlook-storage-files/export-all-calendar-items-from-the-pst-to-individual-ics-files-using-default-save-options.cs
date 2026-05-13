using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;
using System.Collections.Generic;

namespace AsposeEmailPstCalendarExport
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Path to the PST file
                string pstPath = "sample.pst";
                // Directory where .ics files will be saved
                string outputDirectory = "CalendarExport";

                // Verify that the PST file exists
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"PST file not found: {pstPath}");
                    return;
                }

                // Ensure the output directory exists
                try
                {
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }

                // Open the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Start processing from the root folder
                    ProcessFolder(pst, pst.RootFolder, outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        private static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputDirectory)
        {
            // Export calendar items in the current folder
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                try
                {
                    // Extract the full MAPI message
                    using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                    {
                        // Check if the message is a calendar item
                        if (mapiMessage.SupportedType == MapiItemType.Calendar)
                        {
                            // Convert to a strongly typed MapiCalendar
                            IMapiMessageItem mapItem = mapiMessage.ToMapiMessageItem();
                            MapiCalendar calendar = mapItem as MapiCalendar;
                            if (calendar != null)
                            {
                                using (calendar)
                                {
                                    // Build a safe file name based on the subject
                                    string subject = string.IsNullOrWhiteSpace(calendar.Subject) ? "Untitled" : calendar.Subject;
                                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                                    {
                                        subject = subject.Replace(invalidChar, '_');
                                    }
                                    string icsFilePath = Path.Combine(outputDirectory, $"{subject}_{Guid.NewGuid():N}.ics");

                                    // Save the calendar as an .ics file using default options
                                    calendar.Save(icsFilePath);
                                    Console.WriteLine($"Exported calendar to: {icsFilePath}");
                                }
                            }
                        }
                    }
                }
                catch (Exception msgEx)
                {
                    Console.Error.WriteLine($"Failed to export message (EntryId: {messageInfo.EntryIdString}): {msgEx.Message}");
                }
            }

            // Recursively process subfolders
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(pst, subFolder, outputDirectory);
            }
        }
    }
}
