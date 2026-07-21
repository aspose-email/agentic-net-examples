using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            string imagePath = "1.jpg";
            string outputPath = "EmbeddedImage_out.msg";

            if (!File.Exists(imagePath))
            {
                byte[] placeholderJpeg = new byte[] { 0xFF, 0xD8, 0xFF, 0xD9 };
                File.WriteAllBytes(imagePath, placeholderJpeg);
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            MailMessage email = new MailMessage();
            email.From = "AndrewIrwin@from.com";
            email.To.Add("SusanMarc@to.com");
            email.Subject = "This is an email";

            AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                "This is my plain text content", new ContentType("text/plain"));

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                "Here is an embedded image. <img src=cid:barcode>", new ContentType("text/html"));

            LinkedResource barcode = new LinkedResource(imagePath, new ContentType("image/jpeg"))
            {
                ContentId = "barcode"
            };

            email.LinkedResources.Add(barcode);
            email.AlternateViews.Add(plainView);
            email.AlternateViews.Add(htmlView);

            email.Save(outputPath, SaveOptions.DefaultMsgUnicode);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
