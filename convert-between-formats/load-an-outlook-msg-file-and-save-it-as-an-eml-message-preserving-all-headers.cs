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
            // Author note: Example demonstrates MSG to EML conversion preserving headers.
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

            // Load the Outlook MSG file
            MapiMessage mapiMessage = MapiMessage.Load(inputPath);

            // Convert to MailMessage preserving all properties
            MailConversionOptions conversionOptions = new MailConversionOptions();

            using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
            {
                // Save as EML; the library infers format from the file extension
                mailMessage.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
