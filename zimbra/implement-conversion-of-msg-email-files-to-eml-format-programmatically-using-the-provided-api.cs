using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Paths for input MSG and output EML files
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

                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file into a MapiMessage
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Convert MapiMessage to MailMessage with default conversion options
                MailConversionOptions convOptions = new MailConversionOptions();
                using (MailMessage mail = msg.ToMailMessage(convOptions))
                {
                    // Save the MailMessage as an EML file
                    EmlSaveOptions emlOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat);
                    mail.Save(outputPath, emlOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
