using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Paths for the source Thunderbird mbox file and the target PST file
            string mboxFilePath = "thunderbird.mbox";
            string pstFilePath = "converted.pst";

            // Verify that the input mbox file exists
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {mboxFilePath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(pstFilePath);
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

            // Convert the mbox file to PST format
            try
            {
                MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath);
                Console.WriteLine($"Conversion successful. PST saved to: {pstFilePath}");
            }
            catch (Exception convEx)
            {
                Console.Error.WriteLine($"Error during conversion: {convEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
