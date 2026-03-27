using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define source MBOX file and destination PST file paths
            string mboxFilePath = "input.mbox";
            string pstFilePath = "output.pst";

            // Verify that the source MBOX file exists
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"Error: MBOX file not found – {mboxFilePath}");
                return;
            }

            // Ensure the output directory exists
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {pstDirectory}. {dirEx.Message}");
                    return;
                }
            }

            // Perform the conversion from MBOX to PST
            using (PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath))
            {
                // The conversion method returns a PersonalStorage object.
                // Additional operations on pstStorage can be performed here if needed.
                Console.WriteLine("Conversion completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
