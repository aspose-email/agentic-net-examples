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
            string outputDirectory = "ExportedMessages";

            // Ensure PST file exists; create a minimal placeholder if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists.
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                ProcessFolder(pst, pst.RootFolder, outputDirectory);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputDirectory)
    {
        // Export messages with non‑empty subjects.
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            if (string.IsNullOrEmpty(messageInfo.Subject))
            {
                Console.Error.WriteLine($"Message with EntryId '{messageInfo.EntryIdString}' has an empty subject. Skipping export.");
                continue;
            }

            try
            {
                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                {
                    // Build a safe file name from the subject.
                    string safeSubject = string.Concat(message.Subject.Split(Path.GetInvalidFileNameChars()));
                    if (string.IsNullOrWhiteSpace(safeSubject))
                    {
                        safeSubject = Guid.NewGuid().ToString();
                    }

                    string filePath = Path.Combine(outputDirectory, $"{safeSubject}.msg");
                    message.Save(filePath);
                    Console.WriteLine($"Exported: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to export message '{messageInfo.EntryIdString}': {ex.Message}");
            }
        }

        // Recursively process subfolders.
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, outputDirectory);
        }
    }
}
