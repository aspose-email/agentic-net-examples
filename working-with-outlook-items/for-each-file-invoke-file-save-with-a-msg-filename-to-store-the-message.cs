using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output directories
            string inputDirectory = "InputFiles";
            string outputDirectory = "OutputMsg";

            // Ensure input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory '{inputDirectory}' does not exist.");
                return;
            }

            // Ensure output directory exists or create it
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory '{outputDirectory}': {dirEx.Message}");
                return;
            }

            // Get all .eml files in the input directory
            string[] emlFiles;
            try
            {
                emlFiles = Directory.GetFiles(inputDirectory, "*.eml");
            }
            catch (Exception getFilesEx)
            {
                Console.Error.WriteLine($"Failed to enumerate files in '{inputDirectory}': {getFilesEx.Message}");
                return;
            }

            foreach (string emlPath in emlFiles)
            {
                // Guard against missing file (should not happen from GetFiles, but double‑check)
                if (!File.Exists(emlPath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found: {emlPath}");
                    continue;
                }

                try
                {
                    // Load the EML file into a MailMessage
                    using (MailMessage mailMessage = MailMessage.Load(emlPath))
                    {
                        // Prepare the .msg output path
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(emlPath);
                        string msgPath = Path.Combine(outputDirectory, fileNameWithoutExt + ".msg");

                        // Save as MSG using Unicode format
                        MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                        mailMessage.Save(msgPath, saveOptions);

                        Console.WriteLine($"Saved MSG: {msgPath}");
                    }
                }
                catch (Exception fileEx)
                {
                    Console.Error.WriteLine($"Error processing '{emlPath}': {fileEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
