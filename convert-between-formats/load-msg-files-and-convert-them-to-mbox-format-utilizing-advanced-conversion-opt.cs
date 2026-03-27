using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // Define input directory containing MSG files and output MBOX file path
            string inputDirectory = "InputMsgs";
            string outputMboxPath = "output.mbox";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Error: Directory not found – {inputDirectory}");
                return;
            }

            // Ensure the directory for the output file exists
            string outputDirectory = Path.GetDirectoryName(outputMboxPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {ex.Message}");
                    return;
                }
            }

            // Configure MBOX save options (advanced options can be set here)
            MboxSaveOptions mboxSaveOptions = new MboxSaveOptions();
            mboxSaveOptions.FromShouldBeEscaped = true; // example advanced option

            // Create the MBOX writer
            using (MboxrdStorageWriter mboxWriter = new MboxrdStorageWriter(outputMboxPath, mboxSaveOptions))
            {
                // Process each MSG file in the input directory
                string[] msgFiles = Directory.GetFiles(inputDirectory, "*.msg");
                foreach (string msgFilePath in msgFiles)
                {
                    try
                    {
                        // Load the MSG file as a MapiMessage
                        MapiMessage mapiMessage = MapiMessage.Load(msgFilePath);

                        // Convert MapiMessage to MailMessage with conversion options preserving metadata
                        MailConversionOptions conversionOptions = new MailConversionOptions();
                        MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions);

                        // Write the MailMessage to the MBOX storage
                        mboxWriter.WriteMessage(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing file '{msgFilePath}': {ex.Message}");
                        // Continue with next file
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}