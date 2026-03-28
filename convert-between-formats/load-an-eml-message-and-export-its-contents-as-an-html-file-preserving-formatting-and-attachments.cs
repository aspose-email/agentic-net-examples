using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";
            string htmlPath = "sample.html";

            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Input file '{emlPath}' not found.");
                return;
            }

            using (MailMessage message = MailMessage.Load(emlPath))
            {
                HtmlSaveOptions options = new HtmlSaveOptions
                {
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                };
                message.Save(htmlPath, options);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
