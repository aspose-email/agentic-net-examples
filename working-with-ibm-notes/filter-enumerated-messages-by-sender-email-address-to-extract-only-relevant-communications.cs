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
            // Path to the PST file
            string pstPath = "sample.pst";

            // Verify the PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Directory to store filtered messages
            string outputDir = "FilteredMessages";

            // Ensure the output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Email address to filter by
            string targetSender = "sender@example.com";

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Access the Inbox folder (standard folder name may vary)
                FolderInfo inbox = pst.RootFolder.GetSubFolder("Inbox");

                // Enumerate all messages in the Inbox
                foreach (MapiMessage msg in inbox.EnumerateMapiMessages())
                {
                    // Ensure each message is disposed after processing
                    using (msg)
                    {
                        try
                        {
                            // Check if the sender matches the target address
                            if (!string.IsNullOrEmpty(msg.SenderEmailAddress) &&
                                string.Equals(msg.SenderEmailAddress, targetSender, StringComparison.OrdinalIgnoreCase))
                            {
                                // Prepare a safe filename based on the subject
                                string safeSubject = string.IsNullOrEmpty(msg.Subject) ? "NoSubject" : msg.Subject;
                                foreach (char c in Path.GetInvalidFileNameChars())
                                {
                                    safeSubject = safeSubject.Replace(c, '_');
                                }

                                // Create a unique file name to avoid collisions
                                string outputPath = Path.Combine(outputDir, $"{safeSubject}_{Guid.NewGuid()}.msg");

                                // Save the filtered message
                                msg.Save(outputPath);
                                Console.WriteLine($"Saved message from {targetSender}: {outputPath}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error processing a message: {ex.Message}");
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
