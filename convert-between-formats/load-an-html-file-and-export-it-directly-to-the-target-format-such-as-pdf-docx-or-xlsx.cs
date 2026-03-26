using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.html";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file '{inputPath}' not found.");
                return;
            }

            using (MailMessage message = MailMessage.Load(inputPath))
            {
                string outputPath = "output.mhtml";
                // Export the loaded HTML email to MHTML format
                message.Save(outputPath, SaveOptions.DefaultMhtml);
                Console.WriteLine($"Converted '{inputPath}' to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}