using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace ValidatePstAttachmentFilenames
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the PST file to be validated
                string pstFilePath = "input.pst";

                // Directory where extracted attachments will be saved
                string attachmentOutputDir = "ExtractedAttachments";

                // Guard against missing PST file
                if (!File.Exists(pstFilePath))
                {
                    Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                    return;
                }

                // Ensure the output directory exists
                try
                {
                    if (!Directory.Exists(attachmentOutputDir))
                    {
                        Directory.CreateDirectory(attachmentOutputDir);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory '{attachmentOutputDir}': {dirEx.Message}");
                    return;
                }

                // Open the PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    // Iterate through each folder in the PST root
                    foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                    {
                        // Enumerate all messages in the current folder
                        foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                        {
                            // Extract the full message as a MapiMessage
                            using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                            {
                                // Process each attachment in the message
                                foreach (MapiAttachment attachment in mapiMessage.Attachments)
                                {
                                    // Preserve the original attachment filename
                                    string originalFileName = attachment.FileName;

                                    // Build the full path for saving the attachment
                                    string savedAttachmentPath = Path.Combine(attachmentOutputDir, originalFileName);

                                    // Save the attachment to disk
                                    try
                                    {
                                        attachment.Save(savedAttachmentPath);
                                        Console.WriteLine($"Attachment '{originalFileName}' from message '{mapiMessage.Subject}' saved to '{savedAttachmentPath}'.");
                                    }
                                    catch (Exception attachEx)
                                    {
                                        Console.Error.WriteLine($"Failed to save attachment '{originalFileName}': {attachEx.Message}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
