using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string outputPath = "EmbeddedImageMessage.msg";
            string imagePath = "image.png";

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Create the mail message
            using (MailMessage mail = new MailMessage())
            {
                mail.From = "sender@example.com";
                mail.To.Add("recipient@example.com");
                mail.Subject = "Message with Embedded Image";
                // HTML body referencing the image via Content-ID
                mail.HtmlBody = "<html><body><h1>Hello</h1><p>Here is an embedded image:</p><img src=\"cid:myImage\"/></body></html>";

                // Add the image as a linked resource if the file exists
                if (File.Exists(imagePath))
                {
                    try
                    {
                        byte[] imageBytes = File.ReadAllBytes(imagePath);
                        using (MemoryStream imgStream = new MemoryStream(imageBytes))
                        {
                            LinkedResource resource = new LinkedResource(imgStream, "image/png")
                            {
                                ContentId = "myImage"
                            };
                            mail.LinkedResources.Add(resource);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to load image: {ex.Message}");
                        // Continue without the image
                    }
                }
                else
                {
                    Console.Error.WriteLine("Image file not found; proceeding without embedding an image.");
                }

                // Convert to MAPI message and save as MSG
                using (MapiMessage mapiMsg = MapiMessage.FromMailMessage(mail))
                {
                    try
                    {
                        mapiMsg.Save(outputPath);
                        Console.WriteLine($"Message saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
