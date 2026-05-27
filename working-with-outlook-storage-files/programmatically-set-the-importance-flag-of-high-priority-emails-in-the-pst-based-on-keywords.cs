using System;
using System.Collections.Generic;
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
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode)) { }
                Console.WriteLine($"Created placeholder PST file at: {pstPath}");
            }

            // Keywords that indicate high priority
            List<string> keywords = new List<string> { "Urgent", "Action Required" };

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Breadth‑first traversal of all folders
                Queue<FolderInfo> folders = new Queue<FolderInfo>();
                folders.Enqueue(pst.RootFolder);

                while (folders.Count > 0)
                {
                    FolderInfo folder = folders.Dequeue();

                    // Enqueue subfolders
                    foreach (FolderInfo subFolder in folder.GetSubFolders())
                    {
                        folders.Enqueue(subFolder);
                    }

                    // Process each message in the current folder
                    foreach (MessageInfo msgInfo in folder.EnumerateMessages())
                    {
                        try
                        {
                            // Extract the MAPI message
                            using (MapiMessage mapiMsg = pst.ExtractMessage(msgInfo))
                            {
                                // Determine if the message matches any keyword
                                bool isHighPriority = false;
                                foreach (string kw in keywords)
                                {
                                    if (!string.IsNullOrEmpty(mapiMsg.Subject) &&
                                        mapiMsg.Subject.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        isHighPriority = true;
                                        break;
                                    }

                                    if (!string.IsNullOrEmpty(mapiMsg.Body) &&
                                        mapiMsg.Body.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0)
                                    {
                                        isHighPriority = true;
                                        break;
                                    }
                                }

                                if (!isHighPriority)
                                    continue;

                                // Convert to MailMessage with required options
                                MailConversionOptions convOptions = new MailConversionOptions();
                                MailMessage mail = mapiMsg.ToMailMessage(convOptions);

                                // Set priority to High
                                mail.Priority = MailPriority.High;

                                // Convert back to MAPI message
                                MapiMessage updatedMapi = MapiMessage.FromMailMessage(mail);

                                // Update the message inside the PST
                                folder.UpdateMessage(msgInfo.EntryIdString, updatedMapi);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to process message ID {msgInfo.EntryIdString}: {ex.Message}");
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
