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
            // Create an AMP message
            using (AmpMessage message = new AmpMessage())
            {
                // Create an AMP carousel with width and height
                AmpCarousel carousel = new AmpCarousel(600, 400);

                // Add some images to the carousel
                carousel.Images.Add(new AmpImage(600, 400) { Src = "https://example.com/image1.jpg", Alt = "Image 1" });
                carousel.Images.Add(new AmpImage(600, 400) { Src = "https://example.com/image2.jpg", Alt = "Image 2" });
                carousel.Images.Add(new AmpImage(600, 400) { Src = "https://example.com/image3.jpg", Alt = "Image 3" });

                // Add the carousel to the message
                message.AddAmpComponent(carousel);

                // Index of the slide to remove
                int indexToRemove = 1; // remove the second slide (zero‑based)

                // Validate index and remove the slide
                if (indexToRemove >= 0 && indexToRemove < carousel.Images.Count)
                {
                    carousel.Images.RemoveAt(indexToRemove);
                }
                else
                {
                    Console.Error.WriteLine($"Index {indexToRemove} is out of range. No slide removed.");
                }

                // Save the message to a file
                string outputPath = "amp_message.eml";

                try
                {
                    // Ensure the directory exists
                    string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Save the message
                    message.Save(outputPath);
                    Console.WriteLine($"AMP message saved to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
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
