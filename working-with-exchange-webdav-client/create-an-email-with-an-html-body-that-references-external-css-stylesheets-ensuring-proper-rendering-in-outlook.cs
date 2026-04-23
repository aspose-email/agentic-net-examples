using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Prepare output directory and file path
            string outputDirectory = "Output";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string outputFilePath = Path.Combine(outputDirectory, "EmailWithExternalCss.mht");

            // Create the email message
            MailMessage message = new MailMessage(
                "sender@example.com",
                "recipient@example.com",
                "Test Email with External CSS",
                string.Empty);

            // Set HTML body with an external CSS reference
            message.IsBodyHtml = true;
            message.HtmlBody = "<html><head><link rel=\"stylesheet\" href=\"https://example.com/style.css\"></head><body><h1>Hello</h1><p>This email uses external CSS.</p></body></html>";

            // Configure MHTML save options to embed additional CSS styles
            MhtSaveOptions saveOptions = new MhtSaveOptions();
            saveOptions.CssStyles = "h1 { color: blue; } p { font-size: 14px; }";

            // Save the message as MHTML
            using (message)
            {
                message.Save(outputFilePath, saveOptions);
            }

            Console.WriteLine("Email saved successfully to: " + outputFilePath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
