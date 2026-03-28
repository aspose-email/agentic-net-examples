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

            // Verify the input file exists before attempting to load it
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            // Load the message from the file
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Iterate through each AlternateView in the message
                foreach (AlternateView view in message.AlternateViews)
                {
                    // Iterate through each linked resource (embedded image) in the view
                    foreach (LinkedResource resource in view.LinkedResources)
                    {
                        try
                        {
                            // Create a unique file name for the extracted resource
                            string resourceId = string.IsNullOrEmpty(resource.ContentId) ? "resource" : resource.ContentId;
                            string outputPath = $"{resourceId}_{Guid.NewGuid()}.msg";

                            // Save the resource's content stream to a MSG file
                            using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                            {
                                if (resource.ContentStream != null)
                                {
                                    resource.ContentStream.CopyTo(fileStream);
                                }
                            }

                            Console.WriteLine($"Saved linked resource to '{outputPath}'.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save linked resource: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
