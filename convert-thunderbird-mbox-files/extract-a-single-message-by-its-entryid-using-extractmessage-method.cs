using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the PST file
            string pstPath = "sample.pst";
            // EntryId of the message to extract (replace with actual value)
            string entryId = "YOUR_ENTRY_ID";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Open the PST storage
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Extract the message by its EntryId
                MapiMessage extractedMessage = pst.ExtractMessage(entryId);

                // Prepare output path
                string outputPath = "extracted.msg";
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the extracted message
                extractedMessage.Save(outputPath);
                Console.WriteLine($"Message extracted and saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
