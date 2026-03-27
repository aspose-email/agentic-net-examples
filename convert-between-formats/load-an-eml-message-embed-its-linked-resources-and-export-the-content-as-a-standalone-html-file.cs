using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string emlPath = "sample.eml";
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {emlPath}");
                return;
            }

            using (MailMessage message = MailMessage.Load(emlPath))
            {
                HtmlSaveOptions saveOptions = new HtmlSaveOptions()
                {
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                };

                string htmlPath = emlPath + ".html";
                try
                {
                    message.Save(htmlPath, saveOptions);
                    Console.WriteLine($"HTML saved to {htmlPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving HTML: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
