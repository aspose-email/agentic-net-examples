using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Create an AMP message instance
            using (AmpMessage ampMessage = new AmpMessage())
            {
                // Initialize an AMP carousel with desired dimensions
                AmpCarousel carousel = new AmpCarousel(600, 400);

                // Create first image and set its properties
                AmpImage firstImage = new AmpImage(300, 200);
                firstImage.Src = "https://example.com/image1.jpg";
                firstImage.Alt = "First image";

                // Create second image and set its properties
                AmpImage secondImage = new AmpImage(300, 200);
                secondImage.Src = "https://example.com/image2.jpg";
                secondImage.Alt = "Second image";

                // Add images to the carousel
                carousel.Images.Add(firstImage);
                carousel.Images.Add(secondImage);

                // Modify the caption (Alt text) of the second image
                int targetIndex = 1; // zero‑based index for the second image
                if (targetIndex >= 0 && targetIndex < carousel.Images.Count)
                {
                    carousel.Images[targetIndex].Alt = "Updated caption for second image";
                }

                // Add the carousel component to the AMP message
                ampMessage.AddAmpComponent(carousel);

                // Set the AMP HTML body of the message
                ampMessage.AmpHtmlBody = carousel.ToAmpHtml();

                // Output the generated AMP HTML to the console
                Console.WriteLine(ampMessage.AmpHtmlBody);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
