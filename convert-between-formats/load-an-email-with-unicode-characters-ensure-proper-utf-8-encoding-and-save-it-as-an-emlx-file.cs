using System;
using System.IO;
using System.Text;
using Aspose.Email;

namespace EmailConversion
{
    // Author: Aspose.Email example for loading Unicode email and saving as EMLX
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output file paths
                string inputPath = "input.eml";
                string outputPath = "output.emlx";

                // Verify that the input file exists
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

                // Configure load options to enforce UTF-8 encoding
                EmlLoadOptions loadOptions = new EmlLoadOptions
                {
                    PreferredTextEncoding = Encoding.UTF8
                };

                // Load the email message with the specified options
                using (MailMessage message = MailMessage.Load(inputPath, loadOptions))
                {
                    // Use default EMLX save options
                    SaveOptions saveOptions = SaveOptions.DefaultEmlx;

                    // Save the message as an EMLX file
                    message.Save(outputPath, saveOptions);
                }

                Console.WriteLine($"Email successfully saved as EMLX to: {outputPath}");
            }
            catch (Exception ex)
            {
                // Log any unexpected errors
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
