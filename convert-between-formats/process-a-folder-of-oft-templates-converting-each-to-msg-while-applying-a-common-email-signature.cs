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
            // Author note: This example processes all .oft files in a folder,
            // converts each to .msg format and appends a common signature.

            string inputFolder = "Templates"; // folder containing OFT templates
            string outputFolder = "Converted";

            // Ensure input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                return;
            }

            // Create output folder if it does not exist
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Define the common email signature
            const string signature = "\n--\nYour Company Signature";

            // Process each .oft file in the input folder
            string[] oftFiles = Directory.GetFiles(inputFolder, "*.oft");
            foreach (string oftPath in oftFiles)
            {
                try
                {
                    // Load the OFT template as a MapiMessage
                    MapiMessage mapMsg = MapiMessage.Load(oftPath);

                    // Convert to MailMessage using required MailConversionOptions
                    MailMessage mailMsg = mapMsg.ToMailMessage(new MailConversionOptions());

                    // Append the signature to the body (plain text)
                    if (!string.IsNullOrEmpty(mailMsg.Body))
                    {
                        mailMsg.Body += signature;
                    }
                    else
                    {
                        mailMsg.Body = signature;
                    }

                    // Determine output .msg file path
                    string outputPath = Path.Combine(outputFolder,
                        Path.GetFileNameWithoutExtension(oftPath) + ".msg");

                    // Save as MSG; format inferred from extension
                    mailMsg.Save(outputPath);

                    // Dispose MailMessage (MapiMessage does not implement IDisposable)
                    mailMsg.Dispose();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to process '{oftPath}': {ex.Message}");
                    // Continue with next file
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
