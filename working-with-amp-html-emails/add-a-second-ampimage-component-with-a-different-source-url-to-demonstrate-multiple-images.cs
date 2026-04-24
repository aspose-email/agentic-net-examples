using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare output directory
            string outputPath = "amp_email.eml";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create an AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = "sender@example.com";
                ampMessage.To.Add("recipient@example.com");
                ampMessage.Subject = "AMP Email with Multiple Images";

                // First image component
                AmpImage image1 = new AmpImage(300, 200)
                {
                    Src = "https://example.com/image1.jpg",
                    Alt = "First image"
                };
                ampMessage.AddAmpComponent(image1);

                // Second image component with a different source URL
                AmpImage image2 = new AmpImage(400, 250)
                {
                    Src = "https://example.com/image2.png",
                    Alt = "Second image"
                };
                ampMessage.AddAmpComponent(image2);

                // Save the message to a file
                try
                {
                    ampMessage.Save(outputPath);
                    Console.WriteLine($"AMP email saved to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save AMP email: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
