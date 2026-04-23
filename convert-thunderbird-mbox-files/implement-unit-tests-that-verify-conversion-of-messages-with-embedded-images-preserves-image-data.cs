using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string baseDir = Path.Combine(Directory.GetCurrentDirectory(), "EmailTest");
            string emlPath = Path.Combine(baseDir, "sample.eml");
            string htmlPath = Path.Combine(baseDir, "sample.html");

            // Ensure base directory exists
            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }

            // Create a sample email with an embedded image if it does not exist
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                CreateSampleEmail(emlPath);
            }

            // Load the email
            using (MailMessage message = MailMessage.Load(emlPath))
            {
                // Convert to HTML with embedded resources
                HtmlSaveOptions saveOptions = new HtmlSaveOptions
                {
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                };

                // Save HTML
                message.Save(htmlPath, saveOptions);
            }

            // Verify that the HTML contains the embedded image data (base64)
            if (File.Exists(htmlPath))
            {
                string htmlContent = File.ReadAllText(htmlPath);
                if (htmlContent.Contains("data:image/png;base64,"))
                {
                    Console.WriteLine("Test Passed: Embedded image data preserved in HTML.");
                }
                else
                {
                    Console.Error.WriteLine("Test Failed: Embedded image data not found in HTML.");
                }
            }
            else
            {
                Console.Error.WriteLine("Test Failed: HTML output file was not created.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Creates a minimal EML file with an embedded PNG image
    private static void CreateSampleEmail(string filePath)
    {
        // 1x1 pixel PNG (transparent) byte array
        byte[] pngBytes = Convert.FromBase64String(
            "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8Xw8AAusB9Y6XK6cAAAAASUVORK5CYII=");

        // Create the mail message
        MailMessage msg = new MailMessage
        {
            From = "sender@example.com",
            To = "receiver@example.com",
            Subject = "Test Email with Embedded Image",
            IsBodyHtml = true,
            HtmlBody = "<html><body><p>Hello</p><img src=\"cid:image1\"></body></html>"
        };

        // Create attachment from the PNG bytes
        using (MemoryStream imgStream = new MemoryStream(pngBytes))
        {
            Attachment imgAttachment = new Attachment(imgStream, "image/png")
            {
                ContentId = "image1",
                Name = "image.png"
            };
            msg.Attachments.Add(imgAttachment);
        }

        // Save the message as EML
        msg.Save(filePath);
    }
}
