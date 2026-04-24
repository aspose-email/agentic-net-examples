using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define input MBOX and output PST paths
            string inputMboxPath = "input.mbox";
            string outputPstPath = "output.pst";

            // Ensure the input MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(inputMboxPath))
            {
                try
                {
                    File.WriteAllText(inputMboxPath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPstPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Convert MBOX to PST
            try
            {
                MailStorageConverter.MboxToPst(inputMboxPath, outputPstPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"MBOX to PST conversion failed: {ex.Message}");
                return;
            }

            // Open the created PST and add a custom property to each message
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(outputPstPath))
                {
                    // Process the root folder and all its subfolders recursively
                    ProcessFolder(pst, pst.RootFolder, inputMboxPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to add custom property to PST: {ex.Message}");
                return;
            }

            Console.WriteLine("Custom PST property added successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively process folders to add the custom property to each message
    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder, string originalMboxName)
    {
        // Enumerate messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
            {
                // Encode the original MBOX file name as Unicode bytes
                byte[] nameBytes = Encoding.Unicode.GetBytes(originalMboxName);

                // Add a custom property named "OriginalMboxFileName"
                mapiMessage.AddCustomProperty(MapiPropertyType.PT_UNICODE, nameBytes, "OriginalMboxFileName");

                // Update the message in the PST with the modified properties
                pst.ChangeMessage(messageInfo.EntryIdString, mapiMessage.Properties);
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder, originalMboxName);
        }
    }
}
