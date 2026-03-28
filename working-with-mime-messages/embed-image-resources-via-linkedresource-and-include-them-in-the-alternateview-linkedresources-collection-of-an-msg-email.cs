using System;
using System.IO;
using System.Net.Mime;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the image and the output MSG file
            string imagePath = "1.jpg";
            string outputPath = "EmbeddedImage_out.msg";

            // Verify that the image file exists before proceeding
            if (!File.Exists(imagePath))
            {
                Console.Error.WriteLine($"Image file '{imagePath}' not found.");
                return;
            }

            // Create the mail message and set basic properties
            using (MailMessage eml = new MailMessage())
            {
                eml.From = "AndrewIrwin@from.com";
                eml.To = "SusanMarc@to.com";
                eml.Subject = "This is an email";

                // Create the plain‑text alternate view
                using (AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain"))
                {
                    // Create the HTML alternate view with a CID reference to the image
                    using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                        "Here is an embedded image. <img src=cid:barcode>", null, "text/html"))
                    {
                        // Create the linked resource (the image) and assign a Content‑Id
                        using (LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
                        {
                            ContentId = "barcode"
                        })
                        {
                            // Add the linked resource to the message
                            eml.LinkedResources.Add(barcode);

                            // Add the alternate views to the message
                            eml.AlternateViews.Add(plainView);
                            eml.AlternateViews.Add(htmlView);

                            // Save the message as an MSG file with Unicode support
                            eml.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
