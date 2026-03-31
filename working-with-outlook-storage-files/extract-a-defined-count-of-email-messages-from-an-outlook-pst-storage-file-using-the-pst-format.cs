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
            const string pstPath = "sample.pst";
            const string outputFolder = "output";
            const int maxMessages = 5;

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Guard PST file existence; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Access the root folder
                FolderInfo rootFolder = pst.RootFolder;

                int extracted = 0;
                foreach (MessageInfo messageInfo in rootFolder.EnumerateMessages())
                {
                    if (extracted >= maxMessages)
                        break;

                    try
                    {
                        using (MapiMessage msg = pst.ExtractMessage(messageInfo))
                        {
                            // Build a safe filename
                            string safeSubject = string.IsNullOrWhiteSpace(msg.Subject) ? "NoSubject" : msg.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                                safeSubject = safeSubject.Replace(c, '_');

                            string fileName = Path.Combine(outputFolder, $"{safeSubject}_{extracted + 1}.msg");
                            msg.Save(fileName);
                            Console.WriteLine($"Saved message: {fileName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error extracting message: {ex.Message}");
                        // Continue with next message
                    }

                    extracted++;
                }

                if (extracted == 0)
                {
                    Console.WriteLine("No messages found in the PST file.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
