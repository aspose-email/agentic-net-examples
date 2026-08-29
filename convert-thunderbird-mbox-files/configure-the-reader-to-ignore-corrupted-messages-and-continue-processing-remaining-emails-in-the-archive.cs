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

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            const string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                FolderInfo rootFolder = pst.RootFolder;
                MessageInfoCollection messages = rootFolder.GetContents(true);

                foreach (MessageInfo messageInfo in messages)
                {
                    try
                    {
                        MapiMessage mapiMsg = pst.ExtractMessage(messageInfo);

                        string subject = string.IsNullOrEmpty(mapiMsg.Subject) ? "NoSubject" : mapiMsg.Subject;
                        foreach (char invalidChar in Path.GetInvalidFileNameChars())
                        {
                            subject = subject.Replace(invalidChar, '_');
                        }

                        string fileName = $"{subject}.msg";
                        string fullPath = Path.Combine(outputDir, fileName);

                        mapiMsg.Save(fullPath);
                        Console.WriteLine($"Saved: {fullPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to process message ID {messageInfo.EntryId}: {ex.Message}");
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
