using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputFolder = "InputMessages";
            string outputFolder = "OutputMessages";

            // Ensure input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                return;
            }

            // Ensure output folder exists or create it
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output folder: {ex.Message}");
                return;
            }

            string[] emlFiles;
            try
            {
                emlFiles = Directory.GetFiles(inputFolder, "*.eml");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                return;
            }

            foreach (string emlPath in emlFiles)
            {
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
                    using (MailMessage message = MailMessage.Load(emlPath))
                    {
                        // Create unique boundary template using a GUID
                        string uniqueBoundary = $"----=_Boundary_{Guid.NewGuid():N}";
                        EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                        {
                            BoundariesTemplate = uniqueBoundary
                        };

                        string fileName = Path.GetFileName(emlPath);
                        string outputPath = Path.Combine(outputFolder, fileName);

                        message.Save(outputPath, saveOptions);
                        Console.WriteLine($"Saved '{fileName}' with unique boundary to '{outputPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{emlPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
