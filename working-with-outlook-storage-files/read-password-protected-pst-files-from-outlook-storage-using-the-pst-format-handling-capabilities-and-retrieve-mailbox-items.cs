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
            string pstPath = "protected.pst";
            string password = "secret";

            // Ensure the PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage placeholder = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Placeholder PST created
                    }
                    Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Load the PST file
            PersonalStorage pst = null;
            try
            {
                pst = PersonalStorage.FromFile(pstPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading PST file: {ex.Message}");
                return;
            }

            using (pst)
            {
                // Verify password if the PST is protected
                if (pst.Store.IsPasswordProtected)
                {
                    if (!pst.Store.IsPasswordValid(password))
                    {
                        Console.Error.WriteLine("Invalid password for the PST file.");
                        return;
                    }
                    Console.WriteLine("Password validated successfully.");
                }

                // Ensure output directory exists
                string outputDir = "output";
                if (!Directory.Exists(outputDir))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                        return;
                    }
                }

                // Iterate through each subfolder and extract messages
                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folder.DisplayName}");
                    foreach (MessageInfo messageInfo in folder.EnumerateMessages())
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");

                        using (MapiMessage message = pst.ExtractMessage(messageInfo))
                        {
                            string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                            // Replace invalid filename characters
                            foreach (char c in Path.GetInvalidFileNameChars())
                            {
                                safeSubject = safeSubject.Replace(c, '_');
                            }
                            string outputPath = Path.Combine(outputDir, $"{safeSubject}.msg");

                            try
                            {
                                message.Save(outputPath);
                                Console.WriteLine($"Saved message to '{outputPath}'.");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                            }
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
