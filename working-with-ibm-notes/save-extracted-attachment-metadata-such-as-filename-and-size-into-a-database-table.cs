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
            const string pstPath = "sample.pst";
            const string csvPath = "attachment_metadata.csv";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode)) { }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Ensure CSV file exists; create header if missing
            if (!File.Exists(csvPath))
            {
                try
                {
                    using (var headerWriter = new StreamWriter(csvPath, false))
                    {
                        headerWriter.WriteLine("FileName,Size");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create metadata CSV: {ex.Message}");
                    return;
                }
            }

            // Open PST and extract attachment metadata
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Process root folder
                    ProcessFolder(pst.RootFolder, pst, csvPath);

                    // Process all subfolders recursively
                    foreach (FolderInfo subFolder in pst.RootFolder.GetSubFolders())
                    {
                        ProcessFolderRecursive(subFolder, pst, csvPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively process a folder and its subfolders
    private static void ProcessFolderRecursive(FolderInfo folder, PersonalStorage pst, string csvPath)
    {
        ProcessFolder(folder, pst, csvPath);
        foreach (FolderInfo sub in folder.GetSubFolders())
        {
            ProcessFolderRecursive(sub, pst, csvPath);
        }
    }

    // Extract attachments from all messages in the given folder
    private static void ProcessFolder(FolderInfo folder, PersonalStorage pst, string csvPath)
    {
        foreach (MessageInfo msgInfo in folder.EnumerateMessages())
        {
            try
            {
                MapiAttachmentCollection attachments = pst.ExtractAttachments(msgInfo);
                foreach (MapiAttachment attachment in attachments)
                {
                    string fileName = attachment.FileName ?? "Unnamed";
                    long size = attachment.BinaryData?.Length ?? 0;

                    try
                    {
                        using (var writer = new StreamWriter(csvPath, true))
                        {
                            writer.WriteLine($"{EscapeCsv(fileName)},{size}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write metadata for '{fileName}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to extract attachments from message '{msgInfo.Subject}': {ex.Message}");
            }
        }
    }

    // Simple CSV escaping for commas and quotes
    private static string EscapeCsv(string value)
    {
        if (value.Contains(",") || value.Contains("\""))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
        return value;
    }
}
