using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.msg";
            string outputPath = "resource.bin";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {ex.Message}");
                    return;
                }
            }

            // Load the MSG email
            MailMessage message;
            try
            {
                message = MailMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Failed to load message – {ex.Message}");
                return;
            }

            // Ensure there is at least one linked resource
            if (message.LinkedResources == null || message.LinkedResources.Count == 0)
            {
                Console.Error.WriteLine("Error: No linked resources found in the message.");
                return;
            }

            // Use the first linked resource for demonstration
            LinkedResource linkedResource = message.LinkedResources[0];
            Stream contentStream = linkedResource.ContentStream;

            if (contentStream == null)
            {
                Console.Error.WriteLine("Error: Linked resource does not contain a content stream.");
                return;
            }

            // Copy the content stream to a file
            try
            {
                using (FileStream outputFile = File.Create(outputPath))
                {
                    contentStream.CopyTo(outputFile);
                }
                Console.WriteLine($"Linked resource saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Failed to write linked resource – {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
