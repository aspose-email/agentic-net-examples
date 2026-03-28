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
            string msgFilePath = "sample.msg";

            // Verify that the input MSG file exists
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                return;
            }

            // Open the MSG file stream and load it into an AmpMessage
            using (FileStream fileStream = File.OpenRead(msgFilePath))
            {
                using (AmpMessage ampMessage = new AmpMessage())
                {
                    // Import the MSG content into the AmpMessage instance
                    ampMessage.Import(fileStream);

                    // Access structured AMP content and other properties
                    Console.WriteLine($"Subject: {ampMessage.Subject}");
                    Console.WriteLine($"From: {ampMessage.From}");
                    Console.WriteLine($"To: {ampMessage.To}");
                    Console.WriteLine("AMP HTML Body:");
                    Console.WriteLine(ampMessage.AmpHtmlBody ?? "(no AMP content)");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
