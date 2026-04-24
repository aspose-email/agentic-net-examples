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
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output.eml");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create an AMP email message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Basic message properties
                ampMessage.From = "sender@example.com";
                ampMessage.To.Add("recipient@example.com");
                ampMessage.Subject = "AMP Email with Tracking Pixel";

                // Create an AMP image component (tracking pixel)
                // Width and height are set to 1 pixel for a typical tracking pixel
                AmpImage trackingPixel = new AmpImage(1, 1)
                {
                    Src = "https://example.com/tracking-pixel.png"
                };

                // Set the click‑through URL using the 'on' attribute
                // The attribute expects a string like "tap:URL"
                trackingPixel.Attributes.On = "tap:https://example.com/click-through";

                // Add the AMP component to the message
                ampMessage.AddAmpComponent(trackingPixel);

                // Optionally set a plain HTML body as fallback
                ampMessage.HtmlBody = "<p>This is a fallback HTML body.</p>";

                // Save the message to a file
                using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    ampMessage.Save(fs, SaveOptions.DefaultEml);
                }

                Console.WriteLine("AMP email with tracking pixel saved to: " + outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
