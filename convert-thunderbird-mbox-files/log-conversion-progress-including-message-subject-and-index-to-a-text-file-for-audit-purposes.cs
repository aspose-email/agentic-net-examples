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
            string pstFilePath = "input.pst";
            string logFilePath = "conversion_audit.txt";

            // Verify PST file exists
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                return;
            }

            // Ensure the directory for the log file exists
            string logDirectory = Path.GetDirectoryName(logFilePath);
            if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
            {
                try
                {
                    Directory.CreateDirectory(logDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create log directory: {dirEx.Message}");
                    return;
                }
            }

            // Open PST and log file within using blocks for proper disposal
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            using (StreamWriter logWriter = new StreamWriter(logFilePath, false))
            {
                int messageIndex = 0;

                // Iterate through all subfolders recursively
                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    ProcessFolder(folder, ref messageIndex, logWriter);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, ref int messageIndex, StreamWriter logWriter)
    {
        // Process messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            messageIndex++;
            string subject = messageInfo.Subject ?? "<no subject>";
            string logEntry = $"Index: {messageIndex}, Subject: {subject}";
            logWriter.WriteLine(logEntry);
            Console.WriteLine(logEntry);
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, ref messageIndex, logWriter);
        }
    }
}
