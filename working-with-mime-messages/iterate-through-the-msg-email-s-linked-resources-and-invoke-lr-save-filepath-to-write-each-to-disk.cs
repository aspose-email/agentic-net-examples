using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path
            string inputMsgPath = "message.msg";

            // Verify the input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Output directory for linked resources
            string outputDir = "LinkedResources";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file into a MailMessage
            using (MailMessage mailMessage = MailMessage.Load(inputMsgPath))
            {
                // Iterate through each linked resource
                int index = 0;
                foreach (LinkedResource lr in mailMessage.LinkedResources)
                {
                    // Build a file name for the resource
                    string fileName = $"resource_{index}";
                    // Try to use ContentId if available
                    if (!string.IsNullOrEmpty(lr.ContentId))
                    {
                        fileName = lr.ContentId;
                    }
                    // Append appropriate extension if known (fallback to .bin)
                    string extension = ".bin";
                    if (!string.IsNullOrEmpty(lr.ContentType?.MediaType))
                    {
                        // Simple mapping for common types
                        if (lr.ContentType.MediaType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase))
                            extension = ".jpg";
                        else if (lr.ContentType.MediaType.Equals("image/png", StringComparison.OrdinalIgnoreCase))
                            extension = ".png";
                        else if (lr.ContentType.MediaType.Equals("image/gif", StringComparison.OrdinalIgnoreCase))
                            extension = ".gif";
                    }
                    string outputPath = Path.Combine(outputDir, fileName + extension);

                    // Save the linked resource to disk
                    using (LinkedResource resource = lr)
                    {
                        resource.Save(outputPath);
                    }

                    Console.WriteLine($"Saved linked resource to: {outputPath}");
                    index++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
