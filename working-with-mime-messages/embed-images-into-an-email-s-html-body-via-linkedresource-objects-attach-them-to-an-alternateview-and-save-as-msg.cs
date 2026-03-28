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

            // Ensure the image file exists; create an empty placeholder if it does not.
            if (!File.Exists(imagePath))
            {
                File.WriteAllBytes(imagePath, new byte[0]);
            }

            // Ensure the output directory exists.
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (MailMessage eml = new MailMessage())
            {
                eml.From = "AndrewIrwin@from.com";
                eml.To = "SusanMarc@to.com";
                eml.Subject = "This is an email";

                using (AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain"))
                {
                    using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                        "Here is an embedded image. <img src=cid:barcode>", null, "text/html"))
                    {
                        using (LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg))
                        {
                            barcode.ContentId = "barcode";
                            htmlView.LinkedResources.Add(barcode);
                        }

                        eml.AlternateViews.Add(plainView);
                        eml.AlternateViews.Add(htmlView);
                    }
                }

                eml.Save(outputPath, SaveOptions.DefaultMsgUnicode);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
