using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string mboxFilePath = "input.mbox";
            string pstFilePath = "output.pst";

            // Ensure the input MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxFilePath))
            {
                try
                {
                    File.WriteAllText(mboxFilePath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST directory: {ex.Message}");
                    return;
                }
            }

            // Initialize conversion options and enable signature removal
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();
            options.RemoveSignature = true;

            // Perform the conversion
            try
            {
                MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath, options);
                Console.WriteLine("MBOX to PST conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
