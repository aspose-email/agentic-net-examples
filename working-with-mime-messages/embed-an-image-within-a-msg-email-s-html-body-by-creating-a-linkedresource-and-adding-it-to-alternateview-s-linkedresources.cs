using System;
using System.IO;
using System.Net.Mime;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Output MSG file path
            string outputPath = "EmbeddedImage_out.msg";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Image to embed
            string imagePath = "1.jpg";
            if (!File.Exists(imagePath))
            {
                try
                {
                    // Create an empty placeholder image file
                    File.WriteAllBytes(imagePath, new byte[0]);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create placeholder image: " + ex.Message);
                    return;
                }
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "AndrewIrwin@from.com";
                message.To.Add("SusanMarc@to.com");
                message.Subject = "This is an email";

                // Plain text view
                using (AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain"))
                {
                    // HTML view with CID reference
                    using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                        "Here is an embedded image.<img src=cid:barcode>", null, "text/html"))
                    {
                        // Linked resource for the image
                        using (LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg))
                        {
                            barcode.ContentId = "barcode";

                            // Add linked resource to the message
                            message.LinkedResources.Add(barcode);

                            // Add alternate views
                            message.AlternateViews.Add(plainView);
                            message.AlternateViews.Add(htmlView);

                            // Save the message as MSG
                            try
                            {
                                message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                                Console.WriteLine("Message saved to " + outputPath);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine("Failed to save message: " + ex.Message);
                                return;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
