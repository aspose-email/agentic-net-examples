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
            string outputDirectory = "MsgOutput";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Open PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Iterate through all subfolders
                    foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                    {
                        // Enumerate messages in the current folder
                        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
                        {
                            try
                            {
                                // Extract original MAPI message
                                using (MapiMessage originalMessage = pst.ExtractMessage(messageInfo))
                                {
                                    // Prepare MSG file path
                                    string safeSubject = string.IsNullOrWhiteSpace(messageInfo.Subject) ? "Untitled" : messageInfo.Subject;
                                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                                    {
                                        safeSubject = safeSubject.Replace(invalidChar, '_');
                                    }
                                    string msgFilePath = Path.Combine(outputDirectory, $"{safeSubject}.msg");

                                    // Save as MSG
                                    originalMessage.Save(msgFilePath);

                                    // Load the saved MSG
                                    using (MapiMessage savedMessage = MapiMessage.Load(msgFilePath))
                                    {
                                        // Compare sender information
                                        bool senderMatch = string.Equals(originalMessage.SenderEmailAddress, savedMessage.SenderEmailAddress, StringComparison.OrdinalIgnoreCase) &&
                                                          string.Equals(originalMessage.SenderName, savedMessage.SenderName, StringComparison.Ordinal);

                                        // Compare submission timestamps
                                        bool timeMatch = originalMessage.ClientSubmitTime == savedMessage.ClientSubmitTime;

                                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                                        Console.WriteLine($"Sender match: {senderMatch}, Timestamp match: {timeMatch}");
                                    }
                                }
                            }
                            catch (Exception msgEx)
                            {
                                Console.Error.WriteLine($"Error processing message '{messageInfo.Subject}': {msgEx.Message}");
                                // Continue with next message
                            }
                        }
                    }
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Failed to open or process PST file: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
