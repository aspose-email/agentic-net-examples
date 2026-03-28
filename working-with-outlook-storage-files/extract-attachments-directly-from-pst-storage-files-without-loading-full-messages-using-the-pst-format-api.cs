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
            // Path to the PST file
            string pstPath = "sample.pst";

            // Verify that the PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Error: PST file not found – {pstPath}");
                return;
            }

            // Ensure the output directory for attachments exists
            string outputDirectory = "Attachments";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Iterate through each subfolder in the root folder
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    // Iterate through each message in the current folder
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        try
                        {
                            // Extract attachments for the current message without loading the full message
                            MapiAttachmentCollection attachments = pst.ExtractAttachments(messageInfo);

                            // Save each attachment to the output directory
                            foreach (MapiAttachment attachment in attachments)
                            {
                                string attachmentPath = Path.Combine(outputDirectory, attachment.FileName);
                                attachment.Save(attachmentPath);
                                Console.WriteLine($"Saved attachment: {attachmentPath}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error processing message '{messageInfo.Subject}': {ex.Message}");
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
