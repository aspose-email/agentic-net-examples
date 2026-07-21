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
            const string pstPath = "storage.pst";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            const string tempMsgDir = "ExtractedMessages";
            try
            {
                if (!Directory.Exists(tempMsgDir))
                    Directory.CreateDirectory(tempMsgDir);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create temporary directory '{tempMsgDir}': {ex.Message}");
                return;
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                    Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                    Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        Console.WriteLine($"Processing message: {messageInfo.Subject}");

                        // Extract as MapiMessage
                        MapiMessage mapiMsg;
                        try
                        {
                            mapiMsg = pst.ExtractMessage(messageInfo);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to extract message '{messageInfo.Subject}': {ex.Message}");
                            continue;
                        }

                        // Build a safe filename
                        string safeSubject = string.IsNullOrWhiteSpace(mapiMsg.Subject) ? "NoSubject" : mapiMsg.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            safeSubject = safeSubject.Replace(c, '_');

                        string msgFilePath = Path.Combine(tempMsgDir, $"{safeSubject}_{Guid.NewGuid()}.msg");

                        try
                        {
                            mapiMsg.Save(msgFilePath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message '{mapiMsg.Subject}': {ex.Message}");
                            continue;
                        }

                        // Check for distribution list
                        if (mapiMsg.SupportedType == MapiItemType.DistList)
                        {
                            object item = mapiMsg.ToMapiMessageItem();
                            if (item is MapiDistributionList distList)
                            {
                                Console.WriteLine($"Distribution List: {distList.DisplayName}");
                                Console.WriteLine($"Members count: {distList.Members.Count}");
                                foreach (MapiDistributionListMember member in distList.Members)
                                {
                                    Console.WriteLine($" - {member.DisplayName} <{member.EmailAddress}>");
                                }
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
