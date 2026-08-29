using System;
using System.IO;
using Aspose.Email;

namespace OftToRtfConversion
{
    // Author: Aspose.Email example - OFT to RTF conversion preserving styling and embedded objects
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input OFT template path
                const string inputPath = "template.oft";
                // Desired RTF output path
                const string outputPath = "output.rtf";

                // Verify input file exists
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

                // Ensure output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the OFT file preserving RTF body content
                MsgLoadOptions loadOptions = new MsgLoadOptions
                {
                    PreserveRtfContent = true
                };

                using (MailMessage message = MailMessage.Load(inputPath, loadOptions))
                {
                    // Save as RTF; format is inferred from the .rtf extension
                    message.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
