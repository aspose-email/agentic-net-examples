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
            // Output file path
            string outputPath = "AmpEmail.eml";

            // Ensure the output directory exists
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
                ampMessage.Subject = "AMP Email with Image";

                // Create an AMP image component with width and height
                AmpImage ampImage = new AmpImage(300, 200);
                ampImage.Src = "https://example.com/image.jpg";
                ampImage.Alt = "Sample Image";

                // Add the image component to the message
                ampMessage.AddAmpComponent(ampImage);

                // Set a minimal HTML body (required for AMP messages)
                ampMessage.HtmlBody = "<html><head></head><body></body></html>";
                ampMessage.IsBodyHtml = true;

                // Save the message to a file
                ampMessage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
