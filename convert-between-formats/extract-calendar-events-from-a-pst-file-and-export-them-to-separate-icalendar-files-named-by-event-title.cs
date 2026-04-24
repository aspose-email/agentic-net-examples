using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the PST file
            string pstPath = "sample.pst";

            // Verify that the PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Directory where extracted iCalendar files will be saved
            string outputDirectory = "ExportedCalendars";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Process the root folder and all subfolders recursively
                ProcessFolder(pst, pst.RootFolder, outputDirectory);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputDirectory)
    {
        // Iterate through all messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            // Extract the full MAPI message
            using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
            {
                // Check if the message is a calendar item
                if (mapiMessage.SupportedType == MapiItemType.Calendar)
                {
                    // Convert the MAPI message to a MapiCalendar object
                    using (MapiCalendar calendar = (MapiCalendar)mapiMessage.ToMapiMessageItem())
                    {
                        // Determine a safe file name based on the calendar subject
                        string subject = calendar.Subject ?? "Untitled";
                        string safeFileName = Regex.Replace(subject, @"[<>:""/\\|?*]+", "_");

                        // Build the full path for the .ics file
                        string icsFilePath = Path.Combine(outputDirectory, safeFileName + ".ics");

                        // Ensure the file name is unique
                        if (File.Exists(icsFilePath))
                        {
                            int duplicateIndex = 1;
                            while (File.Exists(Path.Combine(outputDirectory, $"{safeFileName}_{duplicateIndex}.ics")))
                            {
                                duplicateIndex++;
                            }
                            icsFilePath = Path.Combine(outputDirectory, $"{safeFileName}_{duplicateIndex}.ics");
                        }

                        // Save the calendar as an iCalendar file
                        calendar.Save(icsFilePath);
                        Console.WriteLine($"Exported: {icsFilePath}");
                    }
                }
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, outputDirectory);
        }
    }
}
