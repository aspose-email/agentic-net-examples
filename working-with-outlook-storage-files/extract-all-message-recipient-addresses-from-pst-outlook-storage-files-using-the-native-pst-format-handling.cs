using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstPath = "sample.pst";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: PST file not found – {pstPath}");
                return;
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");

                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");

                        MapiRecipientCollection recipientCollection = pst.ExtractRecipients(messageInfo);
                        foreach (MapiRecipient recipient in recipientCollection)
                        {
                            string email = recipient.EmailAddress;
                            if (!string.IsNullOrEmpty(email))
                            {
                                Console.WriteLine($"Recipient: {email}");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
