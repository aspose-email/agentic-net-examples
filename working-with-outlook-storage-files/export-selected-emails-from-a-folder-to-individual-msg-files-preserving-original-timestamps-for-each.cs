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
            string outputFolder = "ExportedMessages";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                FolderInfo inboxFolder = pst.RootFolder.GetSubFolder("Inbox");
                if (inboxFolder == null)
                {
                    Console.Error.WriteLine("Inbox folder not found in PST.");
                    return;
                }

                foreach (MessageInfo messageInfo in inboxFolder.EnumerateMessages())
                {
                    try
                    {
                        MapiMessage msg = pst.ExtractMessage(messageInfo);

                        string safeSubject = string.IsNullOrWhiteSpace(msg.Subject) ? "NoSubject" : msg.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        string outputPath = Path.Combine(outputFolder, $"{safeSubject}_{Guid.NewGuid()}.msg");
                        msg.Save(outputPath);
                        Console.WriteLine($"Saved: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to export a message: {ex.Message}");
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
