using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input HTML file path
            string inputPath = "sample.html";
            // Output MSG file path
            string outputPath = "sample.msg";

            // Ensure the input HTML file exists; create a minimal placeholder if missing
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

                const string placeholderHtml = "<html><body><p>Placeholder content</p></body></html>";
                File.WriteAllText(inputPath, placeholderHtml, Encoding.UTF8);
            }

            // Load the HTML document with options to preserve encoding and add a plain‑text view
            HtmlLoadOptions htmlLoadOptions = new HtmlLoadOptions
            {
                PreferredTextEncoding = Encoding.UTF8,
                ShouldAddPlainTextView = true,
                // PathToResources can be set if the HTML references external images; null is acceptable here
                PathToResources = null
            };

            // Load the message from the HTML file
            using (MailMessage message = MailMessage.Load(inputPath, htmlLoadOptions))
            {
                // Prepare MSG save options to preserve original dates and use Unicode format
                MsgSaveOptions msgSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                // Save the message as an Outlook MSG file
                message.Save(outputPath, msgSaveOptions);
            }

            Console.WriteLine("HTML successfully converted to MSG: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
