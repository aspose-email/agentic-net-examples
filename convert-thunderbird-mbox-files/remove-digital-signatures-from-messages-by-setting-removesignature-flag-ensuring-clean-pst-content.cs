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
            string sourcePstPath = "source.pst";
            string destPstPath = "clean.pst";

            // Verify source PST exists; if not, create an empty placeholder PST
            if (!File.Exists(sourcePstPath))
            {
                Console.Error.WriteLine($"Source PST not found at '{sourcePstPath}'. Creating an empty placeholder PST.");
                using (PersonalStorage placeholder = PersonalStorage.Create(sourcePstPath, FileFormatVersion.Unicode))
                {
                    // No content needed
                }
            }

            // Ensure destination directory exists
            string destDirectory = Path.GetDirectoryName(destPstPath);
            if (!string.IsNullOrEmpty(destDirectory) && !Directory.Exists(destDirectory))
            {
                Directory.CreateDirectory(destDirectory);
            }

            // Open source PST and create destination PST
            using (PersonalStorage sourcePst = PersonalStorage.FromFile(sourcePstPath))
            using (PersonalStorage destPst = PersonalStorage.Create(destPstPath, FileFormatVersion.Unicode))
            {
                // Process the root folder (you can extend this to subfolders recursively)
                FolderInfo sourceRoot = sourcePst.RootFolder;
                FolderInfo destRoot = destPst.RootFolder;

                foreach (MessageInfo messageInfo in sourceRoot.EnumerateMessages())
                {
                    // Extract the full message as a MapiMessage
                    MapiMessage originalMessage = sourcePst.ExtractMessage(messageInfo);

                    // Remove digital signature if present
                    MapiMessage unsignedMessage = originalMessage.RemoveSignature();

                    // Add the unsigned message to the destination PST
                    destRoot.AddMessage(unsignedMessage);
                }
            }

            Console.WriteLine("Signature removal completed. Clean PST saved to: " + destPstPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
