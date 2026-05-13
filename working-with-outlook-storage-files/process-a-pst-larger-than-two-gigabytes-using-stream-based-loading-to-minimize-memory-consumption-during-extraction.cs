using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstFilePath = "large.pst";
            string outputDirectory = "ExtractedMessages";

            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (FileStream pstStream = new FileStream(pstFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (PersonalStorage pst = PersonalStorage.FromStream(pstStream, false))
            {
                ProcessFolder(pst.RootFolder, outputDirectory, pst);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, string outputDir, PersonalStorage pst)
    {
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage mapiMsg = pst.ExtractMessage(messageInfo))
                using (MailMessage message = mapiMsg.ToMailMessage(new MailConversionOptions()))
                {
                    string safeSubject = string.IsNullOrWhiteSpace(message.Subject)
                        ? Guid.NewGuid().ToString()
                        : SanitizeFileName(message.Subject);

                    string messageFilePath = Path.Combine(outputDir, $"{safeSubject}.msg");
                    message.Save(messageFilePath);
                    Console.WriteLine($"Saved: {messageFilePath}");
                }
            }
            catch (Exception msgEx)
            {
                Console.Error.WriteLine($"Failed to extract/save message '{messageInfo.Subject}': {msgEx.Message}");
            }
        }

        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            string subFolderPath = Path.Combine(outputDir, SanitizeFileName(subFolder.DisplayName));
            if (!Directory.Exists(subFolderPath))
                Directory.CreateDirectory(subFolderPath);

            ProcessFolder(subFolder, subFolderPath, pst);
        }
    }

    private static string SanitizeFileName(string name)
    {
        foreach (char invalidChar in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(invalidChar.ToString(), "_");
        }

        const int maxLength = 100;
        if (name.Length > maxLength)
            name = name.Substring(0, maxLength);

        return name;
    }
}
