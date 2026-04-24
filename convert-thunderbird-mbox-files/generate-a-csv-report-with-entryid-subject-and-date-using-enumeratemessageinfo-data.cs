using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstFilePath = "sample.pst";
            string csvOutputPath = "report.csv";

            // Guard PST file existence
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(csvOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                using (StreamWriter writer = new StreamWriter(csvOutputPath, false, System.Text.Encoding.UTF8))
                {
                    // Write CSV header
                    writer.WriteLine("EntryId,Subject,Date");

                    // Iterate through all folders recursively
                    foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                    {
                        ProcessFolder(pst, folder, writer);
                    }
                }

                Console.WriteLine($"CSV report generated at: {csvOutputPath}");
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"IO error: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder, StreamWriter writer)
    {
        // Enumerate messages in the current folder
        foreach (MessageInfo msgInfo in folder.EnumerateMessages())
        {
            string entryId = msgInfo.EntryIdString ?? string.Empty;
            string subject = msgInfo.Subject ?? string.Empty;
            string dateString = string.Empty;

            // Extract the full message to obtain the date
            using (MapiMessage fullMessage = pst.ExtractMessage(msgInfo))
            {
                if (fullMessage != null && fullMessage.ClientSubmitTime != DateTime.MinValue)
                {
                    dateString = fullMessage.ClientSubmitTime.ToString("o");
                }
            }

            // Escape commas in subject
            string escapedSubject = subject.Replace("\"", "\"\"");
            if (escapedSubject.Contains(","))
            {
                escapedSubject = $"\"{escapedSubject}\"";
            }

            writer.WriteLine($"{entryId},{escapedSubject},{dateString}");
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, writer);
        }
    }
}
