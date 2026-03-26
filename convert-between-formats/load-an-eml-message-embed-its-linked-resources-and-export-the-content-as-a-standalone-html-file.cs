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
            string emlPath = "EmailWithAttachEmbedded.eml";

            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {emlPath}");
                return;
            }

            using (MailMessage eml = MailMessage.Load(emlPath))
            {
                HtmlSaveOptions options = new HtmlSaveOptions();
                options.ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml;

                string htmlPath = emlPath + ".html";

                eml.Save(htmlPath, options);
                Console.WriteLine($"HTML saved to {htmlPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}