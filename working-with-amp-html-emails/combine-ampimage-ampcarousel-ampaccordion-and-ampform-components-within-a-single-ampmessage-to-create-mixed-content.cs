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
            // Prepare output directory and file path
            string outputDirectory = Path.Combine(Environment.CurrentDirectory, "Output");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            string outputPath = Path.Combine(outputDirectory, "MixedAmpMessage.eml");

            // Create an AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Set basic mail properties
                ampMessage.From = "sender@example.com";
                ampMessage.To.Add("recipient@example.com");
                ampMessage.Subject = "AMP Email with Mixed Components";

                // Create an AMP Image (required width and height)
                AmpImage ampImage = new AmpImage(300, 200);
                ampImage.Src = "https://example.com/image.jpg";
                ampImage.Alt = "Sample Image";

                // Create an AMP Carousel (required width and height)
                AmpCarousel ampCarousel = new AmpCarousel(600, 300);
                // Add images to the carousel
                AmpImage carouselImage1 = new AmpImage(600, 300);
                carouselImage1.Src = "https://example.com/carousel1.jpg";
                AmpImage carouselImage2 = new AmpImage(600, 300);
                carouselImage2.Src = "https://example.com/carousel2.jpg";
                ampCarousel.Images.Add(carouselImage1);
                ampCarousel.Images.Add(carouselImage2);

                // Create an AMP Accordion
                AmpAccordion ampAccordion = new AmpAccordion();

                // Create an AMP Form
                AmpForm ampForm = new AmpForm();
                ampForm.Action = "https://example.com/submit";

                // Add components to the message
                ampMessage.AddAmpComponent(ampImage);
                ampMessage.AddAmpComponent(ampCarousel);
                ampMessage.AddAmpComponent(ampAccordion);
                ampMessage.AddAmpComponent(ampForm);

                // Optionally set a plain text body
                ampMessage.Body = "This email contains AMP components. View it in a compatible client.";

                // Save the message to a file
                ampMessage.Save(outputPath);
                Console.WriteLine("AMP message saved to: " + outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
