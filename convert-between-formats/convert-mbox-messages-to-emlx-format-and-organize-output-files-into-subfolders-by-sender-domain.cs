using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string outputRoot = "output";

            // Guard input file existence
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output root directory exists
            try
            {
                Directory.CreateDirectory(outputRoot);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Create MBOX reader using the required factory method
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                while (true)
                {
                    MailMessage message = reader.ReadNextMessage();
                    if (message == null)
                        break;

                    using (message)
                    {
                        // Determine sender domain
                        string fromAddress = message.From?.Address ?? "unknown@unknown.com";
                        string domain = "unknown";
                        int atIndex = fromAddress.IndexOf('@');
                        if (atIndex >= 0 && atIndex < fromAddress.Length - 1)
                            domain = fromAddress.Substring(atIndex + 1);

                        // Create domain subfolder
                        string domainFolder = Path.Combine(outputRoot, domain);
                        try
                        {
                            Directory.CreateDirectory(domainFolder);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create domain folder '{domainFolder}': {ex.Message}");
                            continue;
                        }

                        // Build a safe file name
                        string subjectPart = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            subjectPart = subjectPart.Replace(c, '_');

                        string fileName = $"{subjectPart}_{Guid.NewGuid():N}.emlx";
                        string outputPath = Path.Combine(domainFolder, fileName);

                        // Save the message as .emlx (treated as .eml format)
                        try
                        {
                            message.Save(outputPath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message to '{outputPath}': {ex.Message}");
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
