using System;
using System.IO;
using System.Text;
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
            string outputDirectory = "ExportedMhtml";

            // Guard PST file existence
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found at path: {pstPath}");
                return;
            }

            // Ensure output directory exists
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

            // Open PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Process root folder and all subfolders recursively
                    ProcessFolder(pst.RootFolder, outputDirectory);
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Error processing PST file: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, string outputDirectory)
    {
        // Export all messages in the current folder
        try
        {
            foreach (MapiMessage mapiMessage in folder.EnumerateMapiMessages())
            {
                ExportMessageToMhtml(mapiMessage, outputDirectory);
            }
        }
        catch (Exception msgEx)
        {
            Console.Error.WriteLine($"Error enumerating messages in folder '{folder.DisplayName}': {msgEx.Message}");
        }

        // Recursively process subfolders
        try
        {
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(subFolder, outputDirectory);
            }
        }
        catch (Exception subEx)
        {
            Console.Error.WriteLine($"Error enumerating subfolders of '{folder.DisplayName}': {subEx.Message}");
        }
    }

    private static void ExportMessageToMhtml(MapiMessage mapiMessage, string outputDirectory)
    {
        // Ensure resources are disposed
        using (MapiMessage message = mapiMessage)
        {
            // Convert to MailMessage
            MailMessage mailMessage;
            try
            {
                mailMessage = message.ToMailMessage(new MailConversionOptions());
            }
            catch (Exception convEx)
            {
                Console.Error.WriteLine($"Failed to convert MAPI message to MailMessage: {convEx.Message}");
                return;
            }

            using (mailMessage)
            {
                // Determine safe file name
                string subject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                string safeFileName = GetSafeFileName(subject) + ".mht";
                string outputPath = Path.Combine(outputDirectory, safeFileName);

                // Save as MHTML
                try
                {
                    MhtSaveOptions saveOptions = new MhtSaveOptions();
                    mailMessage.Save(outputPath, saveOptions);
                    Console.WriteLine($"Saved: {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save MHTML for message '{subject}': {saveEx.Message}");
                }
            }
        }
    }

    private static string GetSafeFileName(string name)
    {
        char[] invalidChars = Path.GetInvalidFileNameChars();
        StringBuilder sb = new StringBuilder(name.Length);
        foreach (char c in name)
        {
            sb.Append(Array.IndexOf(invalidChars, c) >= 0 ? '_' : c);
        }
        // Trim length to avoid filesystem limits
        string result = sb.ToString();
        return result.Length > 200 ? result.Substring(0, 200) : result;
    }
}
