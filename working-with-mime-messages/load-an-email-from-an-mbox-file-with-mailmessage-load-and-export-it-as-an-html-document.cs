using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string mboxFilePath = "sample.mbox";
            string htmlOutputPath = "email.html";

            // Verify the MBOX file exists before attempting to load.
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {mboxFilePath}");
                return;
            }

            // Load the email message from the MBOX file.
            // MailMessage.Load will attempt to parse the file; if the format is unsupported,
            // an exception will be caught by the outer try/catch.
            using (MailMessage message = MailMessage.Load(mboxFilePath))
            {
                // Prefer the HTML body if available; otherwise fall back to plain text.
                string htmlContent = message.HtmlBody;
                if (string.IsNullOrEmpty(htmlContent))
                {
                    // Simple conversion of plain text to HTML.
                    string plainBody = message.Body ?? string.Empty;
                    htmlContent = $"<pre>{System.Net.WebUtility.HtmlEncode(plainBody)}</pre>";
                }

                // Write the HTML content to the output file.
                try
                {
                    File.WriteAllText(htmlOutputPath, htmlContent);
                    Console.WriteLine($"HTML export completed: {htmlOutputPath}");
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Error writing HTML file: {writeEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
