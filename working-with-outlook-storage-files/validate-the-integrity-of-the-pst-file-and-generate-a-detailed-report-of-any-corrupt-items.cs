using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstFilePath = "sample.pst";

            // Verify PST file exists
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                return;
            }

            // Load PST inside a using block
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                List<string> reportLines = new List<string>();
                try
                {
                    // Start processing from the root folder
                    ProcessFolder(pst, pst.RootFolder, reportLines);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error while processing PST: {ex.Message}");
                    return;
                }

                // Output report to console
                Console.WriteLine("PST Integrity Report:");
                foreach (string line in reportLines)
                {
                    Console.WriteLine(line);
                }

                // Write report to a file
                string reportPath = "PSTIntegrityReport.txt";
                try
                {
                    File.WriteAllLines(reportPath, reportLines);
                    Console.WriteLine($"Report saved to {reportPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write report file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }

    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder, List<string> report)
    {
        // Process messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                {
                    // Message extracted successfully; optionally add info
                    string subject = string.IsNullOrEmpty(messageInfo.Subject) ? "<no subject>" : messageInfo.Subject;
                    report.Add($"OK: \"{subject}\" in folder \"{folder.DisplayName}\"");
                }
            }
            catch (Exception ex)
            {
                // Record corrupted message details
                string subject = string.IsNullOrEmpty(messageInfo.Subject) ? "<no subject>" : messageInfo.Subject;
                report.Add($"CORRUPT: \"{subject}\" in folder \"{folder.DisplayName}\" – {ex.Message}");
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, report);
        }
    }
}
