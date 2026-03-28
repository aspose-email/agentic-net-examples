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
            // Define output file path and ensure its directory exists
            string outputPath = "EmbeddedImage_out.msg";
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create the email message
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = "AndrewIrwin@from.com";
                mailMessage.To.Add("SusanMarc@to.com");
                mailMessage.Subject = "This is an email";

                // Create plain text alternate view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain");

                // Create HTML alternate view with a placeholder for an embedded image
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "Here is an embedded image. <img src=cid:barcode>", null, "text/html");

                // Add linked resource (embedded image) if the file exists
                if (File.Exists("1.jpg"))
                {
                    LinkedResource barcode = new LinkedResource("1.jpg", MediaTypeNames.Image.Jpeg)
                    {
                        ContentId = "barcode"
                    };
                    mailMessage.LinkedResources.Add(barcode);
                }

                // Attach alternate views to the message
                mailMessage.AlternateViews.Add(plainView);
                mailMessage.AlternateViews.Add(htmlView);

                // Save the message as MSG (Unicode) using MsgSaveOptions
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                mailMessage.Save(outputPath, saveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
