using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string htmlPath = "sample.html";
            if (!File.Exists(htmlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {htmlPath}");
                return;
            }

            string oftPath = "output.oft";

            try
            {
                string htmlContent = File.ReadAllText(htmlPath);

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.HtmlBody = htmlContent;

                    using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                    {
                        mapiMessage.SaveAsTemplate(oftPath);
                        Console.WriteLine($"OFT file saved to {oftPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing files: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
