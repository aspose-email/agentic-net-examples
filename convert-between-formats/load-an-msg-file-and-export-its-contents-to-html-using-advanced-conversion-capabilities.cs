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
            string inputPath = "sample.msg";
            string outputPath = "output.html";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "Placeholder Subject",
                    "sender@example.com",
                    "receiver@example.com",
                    "This is a placeholder message."))
                {
                    placeholder.Save(inputPath);
                    Console.WriteLine($"Created placeholder MSG at \"{inputPath}\".");
                }
            }

            // Load the MSG file into a MailMessage.
            using (MailMessage mail = MailMessage.Load(inputPath, new MsgLoadOptions()))
            {
                // Configure advanced HTML save options.
                HtmlSaveOptions saveOptions = new HtmlSaveOptions
                {
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                };

                // Ensure the output directory exists.
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the message as HTML.
                mail.Save(outputPath, saveOptions);
                Console.WriteLine($"Message saved as HTML to \"{outputPath}\".");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
