using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the Thunderbird MBOX file
            string mboxPath = "input.mbox";
            // Desired path for the resulting PST file
            string pstPath = "output.pst";

            // Verify that the source MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Error: MBOX file not found – {mboxPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Convert the MBOX storage to PST
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
            {
                // PST is now created; additional processing can be done here if needed
                Console.WriteLine("MBOX to PST conversion completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
