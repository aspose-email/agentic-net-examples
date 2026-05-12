using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.eml";
            string outputPath = "output.eml";

            // Verify that the input file exists
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the email message safely
            try
            {
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    // Standard HTML footer to append
                    const string htmlFooter = "<br/><hr/><p style=\"font-size:small;color:gray;\">© 2026 MyCompany. All rights reserved.</p>";

                    // Append footer to HTML body if present, otherwise create an HTML body
                    if (message.IsBodyHtml && !string.IsNullOrEmpty(message.HtmlBody))
                    {
                        message.HtmlBody += htmlFooter;
                    }
                    else
                    {
                        // Convert plain text body to HTML and append the footer
                        string escapedBody = System.Net.WebUtility.HtmlEncode(message.Body ?? string.Empty).Replace("\n", "<br/>");
                        message.HtmlBody = escapedBody + htmlFooter;
                        message.IsBodyHtml = true;
                    }

                    // Save the modified message
                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved with footer to: {outputPath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                    }
                }
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load message: {loadEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
