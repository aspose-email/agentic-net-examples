using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: Simple MHTML to OFT conversion using Aspose.Email.
            string inputPath = "input.mht";
            string outputPath = "output.oft";

            // Verify input file exists.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the MHTML message.
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Get default options for Outlook template (OFT) format.
                MsgSaveOptions oftOptions = SaveOptions.DefaultOft;

                // Save the message as an OFT template.
                message.Save(outputPath, oftOptions);
            }

            Console.WriteLine($"Successfully converted '{inputPath}' to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
