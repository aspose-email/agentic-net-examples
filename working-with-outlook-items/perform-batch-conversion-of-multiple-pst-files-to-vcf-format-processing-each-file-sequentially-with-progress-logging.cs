using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            string pstDirectory = "PstFiles";
            string outputRoot = "VcfOutput";

            if (!Directory.Exists(pstDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {pstDirectory}");
                return;
            }

            if (!Directory.Exists(outputRoot))
            {
                try
                {
                    Directory.CreateDirectory(outputRoot);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            string[] pstFiles;
            try
            {
                pstFiles = Directory.GetFiles(pstDirectory, "*.pst");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error enumerating PST files: {ex.Message}");
                return;
            }

            foreach (string pstPath in pstFiles)
            {
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"PST file not found, skipping: {pstPath}");
                    continue;
                }

                Console.WriteLine($"Processing PST: {pstPath}");

                string pstFileNameWithoutExt = Path.GetFileNameWithoutExtension(pstPath);
                string pstOutputDir = Path.Combine(outputRoot, pstFileNameWithoutExt);

                try
                {
                    if (!Directory.Exists(pstOutputDir))
                    {
                        Directory.CreateDirectory(pstOutputDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output subdirectory: {ex.Message}");
                    continue;
                }

                try
                {
                    using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                    {
                        ProcessFolder(pst.RootFolder, pst, pstOutputDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing PST '{pstPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, PersonalStorage pst, string outputDir)
    {
        // Process messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                {
                    if (mapiMessage.SupportedType == MapiItemType.Contact)
                    {
                        // Convert to MapiContact
                        MapiContact mapiContact = (MapiContact)mapiMessage.ToMapiMessageItem();

                        // Build VCF file path
                        string vcfFileName = $"{Path.GetFileNameWithoutExtension(messageInfo.Subject ?? "Contact")}_{messageInfo.EntryIdString}.vcf";
                        string vcfPath = Path.Combine(outputDir, vcfFileName);

                        // Save as VCF
                        mapiContact.Save(vcfPath);
                        Console.WriteLine($"Saved contact to: {vcfPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to process message ID {messageInfo.EntryIdString}: {ex.Message}");
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, pst, outputDir);
        }
    }
}
