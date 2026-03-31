using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "input.msg";
            string outputPath = "output.html";

            // Ensure the input MSG file exists; create a minimal placeholder if missing.
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
                    string inputDir = Path.GetDirectoryName(inputPath);
                    if (!string.IsNullOrEmpty(inputDir) && !Directory.Exists(inputDir))
                    {
                        Directory.CreateDirectory(inputDir);
                    }

                    using (MapiMessage placeholder = new MapiMessage(
                        "Sample Subject",
                        "Sample Body",
                        "sender@example.com",
                        "recipient@example.com"))
                    {
                        placeholder.Save(inputPath);
                    }

                    Console.WriteLine($"Placeholder MSG created at '{inputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists.
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file as a MailMessage and access its HtmlBody.
            using (MailMessage mail = MailMessage.Load(inputPath))
            {
                string htmlBody = mail.HtmlBody;

                if (string.IsNullOrEmpty(htmlBody))
                {
                    Console.WriteLine("The message does not contain an HTML body.");
                }

                try
                {
                    File.WriteAllText(outputPath, htmlBody ?? string.Empty);
                    Console.WriteLine($"HTML content saved to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write HTML output: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
