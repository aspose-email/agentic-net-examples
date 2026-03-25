using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input MSG file path
            string inputMsgPath = "input.msg";
            // Output EML file path
            string outputEmlPath = "output.eml";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputEmlPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {ex.Message}");
                    return;
                }
            }

            // Load the MSG file as a MailMessage
            using (MailMessage mailMessage = MailMessage.Load(inputMsgPath))
            {
                // Save the MailMessage as EML
                mailMessage.Save(outputEmlPath, SaveOptions.DefaultEml);
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}