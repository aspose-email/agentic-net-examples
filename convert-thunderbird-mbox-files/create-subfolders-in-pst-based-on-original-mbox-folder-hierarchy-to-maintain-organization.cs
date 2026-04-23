using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Verify that the source MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // If a PST file already exists at the destination, attempt to delete it
            if (File.Exists(pstPath))
            {
                try
                {
                    File.Delete(pstPath);
                }
                catch (Exception deleteEx)
                {
                    Console.Error.WriteLine($"Unable to delete existing PST file: {deleteEx.Message}");
                    return;
                }
            }

            // Convert the MBOX file to a PST file
            PersonalStorage pst = null;
            try
            {
                pst = MailStorageConverter.MboxToPst(mboxPath, pstPath);
            }
            catch (Exception convertEx)
            {
                Console.Error.WriteLine($"MBOX to PST conversion failed: {convertEx.Message}");
                return;
            }

            if (pst == null)
            {
                Console.Error.WriteLine("Conversion returned a null PST object.");
                return;
            }

            // Work with the PST file
            using (pst)
            {
                // Create a predefined folder (e.g., a custom Inbox) using the correct accessor
                FolderInfo customInbox = pst.CreatePredefinedFolder("MyInbox", StandardIpmFolder.Inbox);

                // Create a regular subfolder hierarchy under the root folder
                FolderInfo importedRoot = pst.RootFolder.AddSubFolder("Imported");

                // Create a nested hierarchy "2023\January" under the Imported folder
                FolderInfo yearFolder = importedRoot.AddSubFolder("2023", true); // createHierarchy = true
                FolderInfo monthFolder = yearFolder.AddSubFolder("January");

                Console.WriteLine("Subfolders created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
