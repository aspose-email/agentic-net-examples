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
            const string inputPath = "input.html";
            const string outputPath = "output.mhtml";

            // Ensure the input HTML file exists; create a minimal placeholder if missing.
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

                try
                {
                    const string placeholderHtml = "<html><body><p>Placeholder content</p></body></html>";
                    File.WriteAllText(inputPath, placeholderHtml, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ex.Message}");
                    return;
                }
            }

            // Load the HTML document as a MailMessage with custom load options.
            HtmlLoadOptions loadOptions = new HtmlLoadOptions
            {
                PreferredTextEncoding = Encoding.UTF8,
                ShouldAddPlainTextView = true,
                // PathToResources can be set if the HTML references external images; omitted here.
            };

            try
            {
                using (MailMessage mail = MailMessage.Load(inputPath, loadOptions))
                {
                    // Configure MHTML save options with custom format flags.
                    MhtSaveOptions saveOptions = new MhtSaveOptions
                    {
                        MhtFormatOptions = MhtFormatOptions.WriteHeader | MhtFormatOptions.WriteOutlineAttachments
                    };

                    // Save the message as MHTML.
                    mail.Save(outputPath, saveOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing email conversion: {ex.Message}");
                return;
            }

            Console.WriteLine($"HTML successfully converted to MHTML: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
