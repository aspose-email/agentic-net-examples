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
            // Paths
            string pstPath = "sample.pst";
            string targetFolderName = "Filtered";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open PST
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Ensure target folder exists under the root folder
                FolderInfo targetFolder;
                try
                {
                    targetFolder = pst.RootFolder.GetSubFolder(targetFolderName);
                }
                catch
                {
                    targetFolder = pst.RootFolder.AddSubFolder(targetFolderName);
                }

                // Iterate through all messages in the root folder
                foreach (MessageInfo msgInfo in pst.RootFolder.EnumerateMessages())
                {
                    MapiMessage mapiMessage;
                    try
                    {
                        mapiMessage = pst.ExtractMessage(msgInfo);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to extract message: {ex.Message}");
                        continue;
                    }

                    // Get sender email address; fallback to empty string if null
                    string senderEmail = mapiMessage.SenderEmailAddress ?? string.Empty;
                    int atIndex = senderEmail.LastIndexOf('@');
                    if (atIndex < 0 || atIndex == senderEmail.Length - 1)
                        continue; // No valid domain

                    string domain = senderEmail.Substring(atIndex + 1);
                    // Check if domain matches the desired one (example: "example.com")
                    if (string.Equals(domain, "example.com", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            // Move the message to the target folder
                            pst.MoveItem(msgInfo, targetFolder);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to move message '{msgInfo.Subject}': {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
