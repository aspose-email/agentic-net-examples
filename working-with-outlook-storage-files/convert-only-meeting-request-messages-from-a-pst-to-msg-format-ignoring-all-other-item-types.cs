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
            string pstPath = "input.pst";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            string outputDirectory = "MeetingRequests";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

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

    static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string outputDirectory)
    {
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                {
                    if (string.Equals(mapiMessage.MessageClass, "IPM.Schedule.Meeting.Request", StringComparison.OrdinalIgnoreCase))
                    {
                        string safeSubject = string.IsNullOrWhiteSpace(mapiMessage.Subject) ? "MeetingRequest" : MakeFileNameSafe(mapiMessage.Subject);
                        string msgPath = Path.Combine(outputDirectory, $"{safeSubject}.msg");
                        mapiMessage.Save(msgPath);
                        Console.WriteLine($"Saved meeting request: {msgPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to process message \"{messageInfo.Subject}\": {ex.Message}");
            }
        }

        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, outputDirectory);
        }
    }

    static string MakeFileNameSafe(string name)
    {
        foreach (char invalidChar in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(invalidChar, '_');
        }
        return name;
    }
}
