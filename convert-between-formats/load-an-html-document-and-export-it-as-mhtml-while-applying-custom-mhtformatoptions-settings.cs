using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for input HTML and output MHTML
            string inputPath = "input.html";
            string outputPath = "output.mht";

            // Verify input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                File.WriteAllText(inputPath, "<html><body><p>Placeholder content</p></body></html>");
                Console.Error.WriteLine($"Input file not found. Created placeholder at '{inputPath}'.");
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the HTML document into a MailMessage
            HtmlLoadOptions loadOptions = new HtmlLoadOptions();
            using (MailMessage mailMessage = MailMessage.Load(inputPath, loadOptions))
            {
                // Configure custom MHT save options
                MhtSaveOptions saveOptions = new MhtSaveOptions();
                saveOptions.MhtFormatOptions = MhtFormatOptions.WriteHeader | MhtFormatOptions.WriteOutlineAttachments;

                // Save the MailMessage as MHTML
                mailMessage.Save(outputPath, saveOptions);
                Console.WriteLine($"MHTML file saved to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
