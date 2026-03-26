using System;
using System.IO;
using Aspose.Email;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input Thunderbird email file (EML format)
                string inputFilePath = "sample.eml";
                // Desired output file (MSG format)
                string outputFilePath = "sample.msg";

                // Verify that the input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Error: File not found – {inputFilePath}");
                    return;
                }

                // Load the EML file into a MailMessage object and save it as MSG
                using (MailMessage mailMessage = MailMessage.Load(inputFilePath))
                {
                    // Save the message using the default MSG save options
                    mailMessage.Save(outputFilePath, SaveOptions.DefaultMsg);
                }

                Console.WriteLine("Conversion completed successfully.");
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors gracefully
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}