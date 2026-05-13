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
            using (PersonalStorage pst = PersonalStorage.FromStream(pstStream))
            {
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Processing folder: {folderInfo.DisplayName}");
                    int totalMessages = folderInfo.ContentCount;
                    const int pageSize = 100;

                    for (int startIndex = 0; startIndex < totalMessages; startIndex += pageSize)
                    {
                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages(startIndex, pageSize))
                        {
                            try
                            {
                                using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                                {
                                    string safeFileName = $"{messageInfo.EntryIdString}.msg";
                                    string outputPath = Path.Combine(outputDirectory, safeFileName);
                                    mapiMessage.Save(outputPath);
                                    Console.WriteLine($"Saved: {outputPath}");
                                }
                            }
                            catch (Exception msgEx)
                            {
                                Console.Error.WriteLine($"Failed to extract/save message '{messageInfo.Subject}': {msgEx.Message}");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
