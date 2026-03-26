using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputPath = "input.msg";
                string outputPath = "output.oft";

                // Verify that the input MSG file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                    return;
                }

                // Load the MSG file and save it as an OFT template preserving all properties
                using (Aspose.Email.Mapi.MapiMessage message = Aspose.Email.Mapi.MapiMessage.Load(inputPath))
                {
                    message.Save(outputPath, Aspose.Email.SaveOptions.DefaultOft);
                }

                Console.WriteLine($"Message saved as template to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}