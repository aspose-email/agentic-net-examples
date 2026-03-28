using System;
using System.IO;
using Aspose.Email;

namespace AsposeEmailMhtmlExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                string htmlPath = "input.html";
                if (!File.Exists(htmlPath))
                {
                    Console.Error.WriteLine($"Input file '{htmlPath}' does not exist.");
                    return;
                }

                string htmlContent;
                try
                {
                    htmlContent = File.ReadAllText(htmlPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error reading HTML file: {ex.Message}");
                    return;
                }

                using (MailMessage message = new MailMessage())
                {
                    message.HtmlBody = htmlContent;

                    MhtSaveOptions mhtOptions = new MhtSaveOptions();
                    mhtOptions.SaveAllHeaders = false;

                    string outputPath = "output.mht";
                    try
                    {
                        message.Save(outputPath, mhtOptions);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving MHTML: {ex.Message}");
                        return;
                    }

                    Console.WriteLine($"MHTML saved to '{outputPath}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
