using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace DeterminePstItemTypes
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "sample.pst";

                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {pstPath}");
                    return;
                }

                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    ProcessFolder(pst.RootFolder, pst);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ProcessFolder(FolderInfo folder, PersonalStorage pst)
        {
            // Process messages in the current folder
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                {
                    MapiItemType itemType = message.SupportedType;
                    Console.WriteLine($"Subject: {messageInfo.Subject}");
                    Console.WriteLine($"Item Type: {itemType}");
                    Console.WriteLine();
                }
            }

            // Recursively process subfolders
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(subFolder, pst);
            }
        }
    }
}
