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
            string pstPath = "sample.pst";

            // Guard file existence and create a minimal PST if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Ensure the directory for the PST exists
                    string pstDirectory = Path.GetDirectoryName(pstPath);
                    if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                    {
                        Directory.CreateDirectory(pstDirectory);
                    }

                    // Create an empty Unicode PST file
                    using (PersonalStorage createdPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Optionally create a default folder structure
                        createdPst.RootFolder.AddSubFolder("Inbox");
                    }

                    Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }

                // No further processing needed for a newly created empty PST
                return;
            }

            // Open existing PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Ensure output directory exists
                string outputDir = "output";
                try
                {
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                    return;
                }

                // Iterate through each subfolder in the PST root
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Processing folder: {folderInfo.DisplayName}");

                    // Enumerate messages within the current folder
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        Console.WriteLine($"  Message subject: {messageInfo.Subject}");

                        // Build a safe file name for the extracted MSG
                        string safeSubject = string.IsNullOrWhiteSpace(messageInfo.Subject) ? "NoSubject" : messageInfo.Subject;
                        foreach (char invalidChar in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(invalidChar, '_');
                        }
                        string msgFilePath = Path.Combine(outputDir, $"{safeSubject}.msg");

                        try
                        {
                            // Extract the message to a MapiMessage object
                            using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                            {
                                // Save the original message
                                mapiMessage.Save(msgFilePath);

                                // Modify the subject
                                mapiMessage.Subject = $"Modified - {mapiMessage.Subject}";

                                // Overwrite the MSG file with the modified message
                                mapiMessage.Save(msgFilePath);
                            }

                            Console.WriteLine($"    Extracted and modified message saved to: {msgFilePath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"    Error processing message '{messageInfo.Subject}': {ex.Message}");
                            // Continue with next message
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
