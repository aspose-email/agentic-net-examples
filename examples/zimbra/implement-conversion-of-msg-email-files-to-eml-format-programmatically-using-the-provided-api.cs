using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input MSG file and output EML file paths
            string inputMsgPath = "input.msg";
            string outputEmlPath = "output.eml";

            // Verify that the input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputEmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {outputDirectory}");
                    Console.Error.WriteLine(dirEx.Message);
                    return;
                }
            }

            // Load the MSG file as a MailMessage and save it as EML
            try
            {
                using (MailMessage message = MailMessage.Load(inputMsgPath))
                {
                    message.Save(outputEmlPath, SaveOptions.DefaultEml);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Error processing files: {ioEx.Message}");
                return;
            }

            Console.WriteLine($"Conversion successful: \"{outputEmlPath}\"");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}