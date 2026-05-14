using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.eml";
            string outputPath = "sample.mht";

            // Ensure the input EML file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                using (MailMessage placeholder = new MailMessage())
                {
                    placeholder.From = "sender@example.com";
                    placeholder.To = "recipient@example.com";
                    placeholder.Subject = "Sample Email with Embedded Image";
                    placeholder.IsBodyHtml = true;
                    placeholder.HtmlBody = "<html><body><h1>Hello</h1><img src=\"cid:image1\"/></body></html>";

                    // 1x1 pixel PNG data
                    byte[] pngBytes = new byte[]
                    {
                        0x89,0x50,0x4E,0x47,0x0D,0x0A,0x1A,0x0A,
                        0x00,0x00,0x00,0x0D,0x49,0x48,0x44,0x52,
                        0x00,0x00,0x00,0x01,0x00,0x00,0x00,0x01,
                        0x08,0x06,0x00,0x00,0x00,0x1F,0x15,0xC4,
                        0x89,0x00,0x00,0x00,0x0A,0x49,0x44,0x41,
                        0x54,0x78,0x9C,0x63,0x60,0x00,0x00,0x00,
                        0x02,0x00,0x01,0xE2,0x21,0xBC,0x33,0x00,
                        0x00,0x00,0x00,0x49,0x45,0x4E,0x44,0xAE,
                        0x42,0x60,0x82
                    };

                    using (MemoryStream imgStream = new MemoryStream(pngBytes))
                    {
                        Attachment imageAttachment = new Attachment(imgStream, "image.png", "image/png");
                        imageAttachment.ContentId = "image1";
                        placeholder.Attachments.Add(imageAttachment);

                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
            }

            // Load the email message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Configure MHT save options to embed resources
                MhtSaveOptions mhtOptions = new MhtSaveOptions();
                mhtOptions.ExtractHTMLBodyResourcesAsAttachments = false; // embed resources
                mhtOptions.SaveAttachments = true; // include attachments

                // Save as MHTML (MHT)
                message.Save(outputPath, mhtOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
