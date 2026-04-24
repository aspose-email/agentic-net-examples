using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare output path
            string outputPath = "amp_message.eml";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create an AMP carousel (width: 600, height: 400)
            AmpCarousel carousel = new AmpCarousel(600, 400);

            // First slide
            AmpImage image1 = new AmpImage(600, 400);
            image1.Src = "https://example.com/image1.jpg";
            image1.Alt = "First slide caption";
            carousel.Images.Add(image1);

            // Second slide
            AmpImage image2 = new AmpImage(600, 400);
            image2.Src = "https://example.com/image2.jpg";
            image2.Alt = "Second slide caption";
            carousel.Images.Add(image2);

            // Third slide
            AmpImage image3 = new AmpImage(600, 400);
            image3.Src = "https://example.com/image3.jpg";
            image3.Alt = "Third slide caption";
            carousel.Images.Add(image3);

            // Create AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com", "Sender");
                ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient"));
                ampMessage.Subject = "AMP Email with Carousel";
                ampMessage.IsBodyHtml = true;
                ampMessage.HtmlBody = "<p>This is a fallback HTML body.</p>";

                // Add the carousel component to the message
                ampMessage.AddAmpComponent(carousel);

                // Optionally set the AMP HTML body directly
                ampMessage.AmpHtmlBody = carousel.ToAmpHtml();

                // Save the message to a file
                using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    ampMessage.Save(fs, SaveOptions.DefaultEml);
                }
            }

            Console.WriteLine("AMP email saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
