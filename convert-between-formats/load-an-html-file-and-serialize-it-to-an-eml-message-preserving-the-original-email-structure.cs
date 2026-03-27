using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "input.html";
            string outputPath = "output.eml";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            HtmlLoadOptions loadOptions = new HtmlLoadOptions();

            using (MailMessage mailMessage = MailMessage.Load(inputPath, loadOptions))
            {
                mailMessage.Save(outputPath);
                Console.WriteLine($"HTML file '{inputPath}' has been saved as EML '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
