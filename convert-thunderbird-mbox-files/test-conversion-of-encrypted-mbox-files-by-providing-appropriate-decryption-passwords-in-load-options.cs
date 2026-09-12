using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        // Path to the (potentially encrypted) MBOX file
        string mboxPath = "encrypted.mbox";

        // Verify that the input MBOX file exists
        if (!File.Exists(mboxPath))
        {
            Console.Error.WriteLine($"Input file not found: {mboxPath}");
            return;
        }

        // Prepare output directory
        string outputDir = Path.Combine(Path.GetDirectoryName(mboxPath) ?? string.Empty, "output");
        try
        {
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
            return;
        }

        // Create load options.
        // If the MBOX is encrypted, provide the password via the constructor (if supported).
        // Adjust the constructor usage according to the Aspose.Email version you reference.
        MboxLoadOptions loadOptions = new MboxLoadOptions(); // new MboxLoadOptions("yourPassword") if supported

        try
        {
            // Create the MBOX reader with the load options
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                int messageIndex = 0;
                MailMessage message;
                // Read messages sequentially
                while ((message = mboxReader.ReadNextMessage()) != null)
                {
                    using (message)
                    {
                        // Determine a safe file name based on the subject
                        string subject = message.Subject ?? string.Empty;
                        foreach (char c in Path.GetInvalidFileNameChars())
                            subject = subject.Replace(c, '_');

                        if (string.IsNullOrWhiteSpace(subject))
                            subject = $"Message_{messageIndex}";

                        string outputPath = Path.Combine(outputDir, $"{subject}.eml");

                        // Save the message as .eml
                        message.Save(outputPath);
                        Console.WriteLine($"Saved: {outputPath}");
                        messageIndex++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred while processing the MBOX file: {ex.Message}");
        }
    }
}
