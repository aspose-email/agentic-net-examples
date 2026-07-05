using System;
using System.IO;
using Aspose.Email;

using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Author note: This sample extracts the raw MIME content of a message from a PST file using its EntryId.
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: <exe> <pstFilePath> <messageEntryId>");
                return;
            }

            string pstFilePath = args[0];
            string entryId = args[1];

            // Verify PST file exists
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                return;
            }

            // Open the PST storage
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Extract the message as a MapiMessage using the provided EntryId
                MapiMessage mapMessage = pst.ExtractMessage(entryId);

                // Convert the MapiMessage to a MailMessage (raw MIME representation)
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mailMessage = mapMessage.ToMailMessage(conversionOptions))
                {
                    // Prepare output file path (use EntryId as part of the filename)
                    string outputFilePath = $"{entryId}.eml";

                    // Ensure the output directory exists
                    string outputDir = Path.GetDirectoryName(outputFilePath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Save the MIME content to an .eml file
                    mailMessage.Save(outputFilePath);
                    Console.WriteLine($"Message saved as raw MIME to: {outputFilePath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
