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
            string inputPath = "sample.msg";
            string outputPath = "sample.eml";

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
                        placeholder.Save(inputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            // Load the MSG file into a MailMessage object
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Save the message in EML format
                message.Save(outputPath);
            }

            Console.WriteLine($"Successfully converted '{inputPath}' to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
