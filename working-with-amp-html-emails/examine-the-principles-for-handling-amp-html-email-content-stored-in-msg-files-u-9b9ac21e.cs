using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Verify that the input MSG file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            // Load the MSG file as an AmpMessage
            using (AmpMessage ampMessage = AmpMessage.Load(inputPath) as AmpMessage)
            {
                if (ampMessage == null)
                {
                    Console.Error.WriteLine("Error: Failed to load the message as an AmpMessage.");
                    return;
                }

                // Display the AMP HTML body (if any)
                Console.WriteLine("AMP HTML Body:");
                Console.WriteLine(ampMessage.AmpHtmlBody ?? "(none)");

                // Example: add an AMP component (optional)
                // AmpComponent component = new AmpComponent("amp4email", "<amp-html><h1>Hello AMP</h1></amp-html>");
                // ampMessage.AddAmpComponent(component);

                // Save the modified message to a new MSG file using a stream and SaveOptions
                using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    ampMessage.Save(fileStream, SaveOptions.DefaultMsg);
                }

                Console.WriteLine($"Message saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}