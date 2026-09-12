using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the source PST file
            string pstPath = "storage.pst";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Directory to store extracted MSG files
            string outputDir = "ExtractedMsg";
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Iterate through each subfolder in the root folder
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    // Enumerate messages in the current folder
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        // ----- Capture original attachment filenames -----
                        MapiAttachmentCollection originalAttachments = pst.ExtractAttachments(messageInfo);
                        List<string> originalNames = new List<string>();
                        foreach (MapiAttachment attachment in originalAttachments)
                        {
                            originalNames.Add(attachment.FileName);
                        }

                        // ----- Extract the message and save as MSG -----
                        using (MapiMessage mapiMsg = pst.ExtractMessage(messageInfo))
                        {
                            using (MailMessage mail = mapiMsg.ToMailMessage(new MailConversionOptions()))
                            {
                                string safeSubject = string.IsNullOrWhiteSpace(mail.Subject) ? "NoSubject" : mail.Subject;
                                // Use EntryId to avoid filename collisions
                                string msgFileName = $"{safeSubject}_{messageInfo.EntryId}.msg";
                                string msgPath = Path.Combine(outputDir, msgFileName);
                                mail.Save(msgPath, SaveOptions.DefaultMsg);

                                // ----- Load the saved MSG and verify attachment filenames -----
                                using (MailMessage loadedMail = MailMessage.Load(msgPath))
                                {
                                    List<string> loadedNames = new List<string>();
                                    foreach (Attachment att in loadedMail.Attachments)
                                    {
                                        loadedNames.Add(att.Name);
                                    }

                                    bool match = originalNames.Count == loadedNames.Count &&
                                                 !originalNames.Except(loadedNames).Any();

                                    if (match)
                                    {
                                        Console.WriteLine($"Message '{mail.Subject}' attachments validated.");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Attachment filename mismatch in message '{mail.Subject}'.");
                                    }
                                }
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
