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
            string inputPath = "input.html";
            string outputPath = "output.oft";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read input file: {ex.Message}");
                return;
            }

            using (MailMessage mail = new MailMessage())
            {
                mail.HtmlBody = htmlContent;

                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mail, MapiConversionOptions.UnicodeFormat))
                {
                    try
                    {
                        mapiMessage.SaveAsTemplate(outputPath);
                        Console.WriteLine($"OFT file saved to: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save OFT file: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}