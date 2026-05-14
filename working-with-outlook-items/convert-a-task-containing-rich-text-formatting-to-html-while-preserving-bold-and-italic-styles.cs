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
            const string inputPath = "input.msg";
            const string outputPath = "output.html";

            // Ensure the input file exists; create a minimal placeholder if it does not.
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
                        placeholder.Save(inputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    // Create a simple mail message with HTML body to simulate rich‑text formatting.
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = "sender@example.com";
                        placeholder.To = "receiver@example.com";
                        placeholder.Subject = "Sample Message";

                        // HTML body with bold and italic formatting.
                        placeholder.IsBodyHtml = true;
                        placeholder.Body = "This is <b>bold</b> and <i>italic</i> text.";

                        // Save as MSG using Unicode format (required by validation rules).
                        var msgSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                        placeholder.Save(inputPath, msgSaveOptions);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the message and convert it to HTML while preserving formatting.
            try
            {
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    var htmlOptions = new HtmlSaveOptions
                    {
                        // Embed resources (images, etc.) directly into the HTML.
                        ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                    };

                    // Save the message as HTML using the Save method with SaveOptions.
                    message.Save(outputPath, htmlOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
                return;
            }

            Console.WriteLine($"Conversion completed successfully. HTML saved to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
