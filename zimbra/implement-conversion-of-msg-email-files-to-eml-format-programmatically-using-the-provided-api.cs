using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "input.msg";
            string outputEmlPath = "output.eml";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputEmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {dirEx.Message}");
                    return;
                }
            }

            // Load MSG file and save as EML
            try
            {
                using (MailMessage message = MailMessage.Load(inputMsgPath))
                {
                    message.Save(outputEmlPath, SaveOptions.DefaultEml);
                }
                Console.WriteLine($"Conversion successful: '{outputEmlPath}'");
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Error during conversion: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}