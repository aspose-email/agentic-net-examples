using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input MBOX file and output PST file paths
            string mboxFilePath = "thunderbird.mbox";
            string pstFilePath = "converted.pst";

            // Verify that the input MBOX file exists
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"Error: Input MBOX file not found – {mboxFilePath}");
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
                    Console.Error.WriteLine($"Error: Unable to create output directory – {pstDirectory}");
                    Console.Error.WriteLine(dirEx.Message);
                    return;
                }
            }

            // Perform the conversion inside a try/catch to handle I/O errors
            try
            {
                // Convert MBOX to PST; the method returns a PersonalStorage instance which must be disposed
                using (PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath))
                {
                    // Conversion succeeded; optionally you can work with pstStorage here
                }

                Console.WriteLine("Conversion completed successfully.");
            }
            catch (Exception conversionEx)
            {
                Console.Error.WriteLine("Error during conversion:");
                Console.Error.WriteLine(conversionEx.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error:");
            Console.Error.WriteLine(ex.Message);
        }
    }
}
