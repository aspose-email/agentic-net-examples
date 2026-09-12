using System;
using System.IO;
using Aspose.Email;

namespace EmailConversionSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Author note: Simple EML to MSG conversion using Aspose.Email.
                string inputPath = "input.eml";
                string outputPath = "output.msg";

                // Verify input file exists before attempting to load.
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

                // Load the EML file into a MailMessage instance.
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    // Save the message as MSG using the default MSG save options.
                    message.Save(outputPath, SaveOptions.DefaultMsg);
                }

                Console.WriteLine($"Conversion completed successfully. MSG saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
