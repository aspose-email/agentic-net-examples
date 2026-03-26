using System;
using System.IO;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Paths for the source MBOX file and the destination PST file
            string inputMboxPath = "input.mbox";
            string outputPstPath = "output.pst";

            // Ensure the source MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(inputMboxPath))
            {
                using (FileStream placeholder = File.Create(inputMboxPath))
                {
                    // Empty placeholder; no content needed for demonstration
                }
                Console.WriteLine("Created placeholder MBOX file: " + inputMboxPath);
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPstPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Configure conversion options (set to true to strip signatures)
            Aspose.Email.Storage.MboxToPstConversionOptions options = new Aspose.Email.Storage.MboxToPstConversionOptions();
            options.RemoveSignature = true;

            // Perform the conversion; the method returns a PersonalStorage object that should be disposed
            Aspose.Email.Storage.Pst.PersonalStorage pst = MailStorageConverter.MboxToPst(inputMboxPath, outputPstPath, options);
            using (pst)
            {
                // Conversion completed; the PST file is now available at outputPstPath
            }

            Console.WriteLine("MBOX to PST conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}