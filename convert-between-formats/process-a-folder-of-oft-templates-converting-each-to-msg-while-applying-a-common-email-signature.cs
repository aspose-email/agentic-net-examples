using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputFolder = @"C:\Templates\Oft";
            string outputFolder = @"C:\Converted\Msg";
            string signatureText = "\r\n--\r\nCompany Signature";

            // Verify input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            // Ensure output folder exists
            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output folder: {ex.Message}");
                    return;
                }
            }

            string[] oftFiles;
            try
            {
                oftFiles = Directory.GetFiles(inputFolder, "*.oft");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error enumerating OFT files: {ex.Message}");
                return;
            }

            foreach (string oftPath in oftFiles)
            {
                if (!File.Exists(oftPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(oftPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found, skipping: {oftPath}");
                    continue;
                }

                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(oftPath);
                string msgPath = Path.Combine(outputFolder, fileNameWithoutExt + ".msg");

                try
                {
                    using (MapiMessage templateMessage = MapiMessage.Load(oftPath))
                    {
                        // Convert to MailMessage for easy body manipulation
                        using (MailMessage mail = templateMessage.ToMailMessage(new MailConversionOptions()))
                        {
                            // Append the common signature
                            mail.Body = (mail.Body ?? string.Empty) + signatureText;

                            // Convert back to MapiMessage
                            using (MapiMessage signedMessage = MapiMessage.FromMailMessage(mail))
                            {
                                // Save as MSG
                                signedMessage.Save(msgPath);
                                Console.WriteLine($"Converted: {oftPath} -> {msgPath}");
                            }
                        }
                    }
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
