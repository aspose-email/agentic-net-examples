using System;
using System.IO;
using Aspose.Email;

namespace GenerateRtfFromMsg
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Author note: This sample loads an MSG file preserving its RTF body and saves it as an RTF file.
                string inputPath = "input.msg";
                string outputPath = "output.rtf";

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
                        placeholder.Save(inputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Input file '{inputPath}' not found.");
                    return;
                }

                // Ensure output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Configure load options to preserve RTF content
                MsgLoadOptions loadOptions = new MsgLoadOptions
                {
                    PreserveRtfContent = true
                };

                // Load the MSG message with the specified options
                using (MailMessage message = MailMessage.Load(inputPath, loadOptions))
                {
                    // Save the message as RTF; format is inferred from the .rtf extension
                    message.Save(outputPath);
                }

                Console.WriteLine($"RTF file saved to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
