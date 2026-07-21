using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: This sample demonstrates adding a custom header to an MSG file.
            string inputPath = "input.msg";
            string outputPath = "output.msg";

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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            try
            {
                // Load the MSG email
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    // Add custom header
                    message.Headers.Add("X-Custom-Header", "MyValue");

                    // Save the modified email as MSG
                    message.Save(outputPath, SaveOptions.DefaultMsg);
                }

                Console.WriteLine($"Custom header added and saved to: {outputPath}");
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"Error processing email: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
