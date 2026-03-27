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
            string inputFile = "sample.msg";
            string outputFile = "template.oft";

            if (!File.Exists(inputFile))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputFile}");
                return;
            }

            using (MapiMessage message = MapiMessage.Load(inputFile))
            {
                // Save as Outlook template (OFT) preserving all properties
                message.Save(outputFile, SaveOptions.DefaultOft);
            }

            Console.WriteLine($"Message saved as OFT template: {outputFile}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
