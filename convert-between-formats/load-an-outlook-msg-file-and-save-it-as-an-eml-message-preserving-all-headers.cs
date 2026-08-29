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
            // Author note: This sample loads an Outlook MSG file and saves it as an EML file preserving all headers.
            string inputPath = "input.msg";
            string outputPath = "output.eml";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the MSG file
            MapiMessage mapiMessage = MapiMessage.Load(inputPath);

            // Convert to MailMessage with default conversion options
            MailConversionOptions conversionOptions = new MailConversionOptions();
            using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
            {
                // Save as EML; default options preserve all headers
                mailMessage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
