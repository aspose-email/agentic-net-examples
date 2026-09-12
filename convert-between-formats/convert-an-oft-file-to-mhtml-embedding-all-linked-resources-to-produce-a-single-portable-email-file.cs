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
            // Input OFT file path
            const string inputPath = "template.oft";
            // Output MHTML file path
            const string outputPath = "output.mhtml";

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

            // Load the Outlook template (OFT) as a MAPI message
            MapiMessage mapMsg = MapiMessage.Load(inputPath);

            // Convert MAPI message to MailMessage for saving
            MailConversionOptions conversionOptions = new MailConversionOptions();
            MailMessage mailMsg = mapMsg.ToMailMessage(conversionOptions);

            // Save as MHTML with embedded resources (default options embed resources)
            mailMsg.Save(outputPath, SaveOptions.DefaultMhtml);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
