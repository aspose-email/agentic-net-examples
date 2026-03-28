using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputFile = "input.eml";
            string outputFile = "output.html";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputFile))
            {
                using (FileStream fs = File.Create(inputFile))
                {
                    string placeholder = "From: placeholder@example.com\r\nTo: placeholder@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(placeholder);
                    fs.Write(bytes, 0, bytes.Length);
                }
                Console.Error.WriteLine($"Input file not found. Created placeholder at '{inputFile}'.");
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputFile);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the EML message
            using (MailMessage emlMessage = MailMessage.Load(inputFile))
            {
                // Configure HTML save options to embed resources
                Aspose.Email.HtmlSaveOptions htmlOptions = new Aspose.Email.HtmlSaveOptions
                {
                    ResourceRenderingMode = Aspose.Email.ResourceRenderingMode.EmbedIntoHtml
                };

                // Save the message as HTML
                emlMessage.Save(outputFile, htmlOptions);
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
