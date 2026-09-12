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
            const string pstPath = "storage.pst";
            const string outputDir = "ExtractedMessages";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            using (var pst = PersonalStorage.FromFile(pstPath))
            {
                foreach (var folderInfo in pst.RootFolder.GetSubFolders())
                {
                    foreach (var messageInfo in folderInfo.EnumerateMessages())
                    {
                        MapiMessage msg;
                        try
                        {
                            msg = pst.ExtractMessage(messageInfo);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to extract message '{messageInfo.Subject}': {ex.Message}");
                            continue;
                        }

                        // Remove all attachments (or modify as needed)
                        if (msg.Attachments != null && msg.Attachments.Count > 0)
                        {
                            msg.Attachments.Clear();
                        }

                        string safeSubject = string.IsNullOrWhiteSpace(msg.Subject) ? "NoSubject" : msg.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            safeSubject = safeSubject.Replace(c, '_');

                        string msgPath = Path.Combine(outputDir, $"{safeSubject}.msg");

                        try
                        {
                            msg.Save(msgPath);
                            Console.WriteLine($"Saved without attachments: {msgPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message to '{msgPath}': {ex.Message}");
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
