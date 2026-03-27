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
            string emlPath = "input.eml";
            string msgPath = "output.msg";

            // Verify input file exists
            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Input file not found: {emlPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the EML message
            using (MailMessage emlMessage = MailMessage.Load(emlPath))
            {
                // Convert to MAPI message
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(emlMessage))
                {
                    // Save as MSG file
                    mapiMessage.Save(msgPath);
                    Console.WriteLine($"Message saved to MSG format at: {msgPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
