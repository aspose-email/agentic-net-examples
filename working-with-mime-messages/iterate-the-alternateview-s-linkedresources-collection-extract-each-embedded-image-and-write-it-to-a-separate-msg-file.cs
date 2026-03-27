using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input message file path
            string inputPath = "sample.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputPath}");
                return;
            }

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

            // Load the mail message
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                int viewIndex = 0;
                foreach (AlternateView alternateView in mailMessage.AlternateViews)
                {
                    int resourceIndex = 0;
                    foreach (LinkedResource linkedResource in alternateView.LinkedResources)
                    {
                        // Build a file name for the embedded resource
                        string resourceId = !string.IsNullOrEmpty(linkedResource.ContentId)
                                            ? linkedResource.ContentId
                                            : $"view{viewIndex}_res{resourceIndex}";
                        string outputPath = Path.Combine(outputDir, $"{resourceId}.msg");

                        // Save the linked resource as a MSG file
                        try
                        {
                            using (LinkedResource resource = linkedResource)
                            {
                                resource.Save(outputPath);
                            }
                            Console.WriteLine($"Saved embedded resource to: {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error saving resource '{resourceId}': {ex.Message}");
                        }

                        resourceIndex++;
                    }
                    viewIndex++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
