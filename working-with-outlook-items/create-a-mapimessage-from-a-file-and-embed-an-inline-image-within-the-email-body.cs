using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define file paths
            string msgPath = "input.msg";
            string imagePath = "image.png";
            string outputPath = "output.msg";

            // Verify input files exist
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            if (!File.Exists(imagePath))
            {
                Console.Error.WriteLine($"Image file not found: {imagePath}");
                return;
            }

            // Load the existing Outlook message
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Read image data
                byte[] imageData = File.ReadAllBytes(imagePath);

                // Add the image as an attachment (inline)
                message.Attachments.Add("image.png", imageData);

                // Save the modified message
                message.Save(outputPath);
                Console.WriteLine($"Message saved with inline image to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
