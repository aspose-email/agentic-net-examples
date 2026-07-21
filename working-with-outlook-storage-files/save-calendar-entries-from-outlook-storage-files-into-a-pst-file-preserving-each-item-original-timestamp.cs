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
            // Input Outlook storage file (e.g., PST) containing calendar entries
            string sourcePstPath = "source.pst";
            // Output PST file where calendar entries will be saved
            string destPstPath = "calendar_output.pst";

            // Verify source file exists
            if (!File.Exists(sourcePstPath))
            {
                Console.Error.WriteLine($"Source PST file not found: {sourcePstPath}");
                return;
            }

            // Ensure destination directory exists
            string destDirectory = Path.GetDirectoryName(destPstPath);
            if (!string.IsNullOrEmpty(destDirectory) && !Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }

            // Open source PST
            using (PersonalStorage sourcePst = PersonalStorage.FromFile(sourcePstPath))
            {
                // Get the Calendar folder from the source PST
                FolderInfo sourceCalendarFolder = sourcePst.GetPredefinedFolder(StandardIpmFolder.Appointments);
                if (sourceCalendarFolder == null)
                {
                    Console.Error.WriteLine("Source PST does not contain a Calendar folder.");
                    return;
                }

                // Create destination PST (Unicode format)
                using (PersonalStorage destPst = PersonalStorage.Create(destPstPath, FileFormatVersion.Unicode))
                {
                    // Ensure a Calendar folder exists in the destination PST
                    FolderInfo destCalendarFolder = destPst.GetPredefinedFolder(StandardIpmFolder.Appointments);
                    if (destCalendarFolder == null)
                    {
                        destCalendarFolder = sourcePst.CreatePredefinedFolder("Calendar", StandardIpmFolder.Appointments);
                    }

                    // Enumerate each calendar item (appointment) in the source folder
                    foreach (MessageInfo messageInfo in sourceCalendarFolder.EnumerateMessages())
                    {
                        // Extract the full MAPI message (appointment)
                        MapiMessage appointmentMessage = sourcePst.ExtractMessage(messageInfo);

                        // Preserve original timestamps by copying relevant properties
                        // ClientSubmitTime, CreationTime, LastModificationTime are standard MAPI properties
                        // They are already part of the extracted message, so no extra handling is required.
                        // If needed, you could explicitly set them like this:
                        // appointmentMessage.ClientSubmitTime = messageInfo.ClientSubmitTime;
                        // appointmentMessage.CreationTime = messageInfo.CreationTime;
                        // appointmentMessage.LastModificationTime = messageInfo.LastModificationTime;

                        // Add the appointment to the destination Calendar folder
                        destCalendarFolder.AddMessage(appointmentMessage);
                    }

                    // Destination PST will be saved when disposed
                }
            }

            Console.WriteLine("Calendar entries have been successfully saved to the PST file.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
