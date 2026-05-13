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
            string imagePath = "1.jpg";
            if (!File.Exists(imagePath))
            {
                Console.Error.WriteLine($"Image file '{imagePath}' not found.");
                return;
            }

            using (MailMessage message = new MailMessage())
            {
                message.From = "AndrewIrwin@from.com";
                message.To = "SusanMarc@to.com";
                message.Subject = "This is an email";

                using (AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain"))
                using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "Here is an embedded image. <img src=cid:barcode>", null, "text/html"))
                using (LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg))
                {
                    barcode.ContentId = "barcode";

                    message.LinkedResources.Add(barcode);
                    message.AlternateViews.Add(plainView);
                    message.AlternateViews.Add(htmlView);

                    string outputPath = "EmbeddedImage_out.msg";
                    try
                    {
                        message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                        Console.WriteLine($"Message saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
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
