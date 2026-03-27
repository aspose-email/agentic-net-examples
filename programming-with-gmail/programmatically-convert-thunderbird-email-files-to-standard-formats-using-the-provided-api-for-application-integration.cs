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
            // Paths for the source Thunderbird MBOX file and the target PST file
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Verify that the source MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Error: File not found – {mboxPath}");
                return;
            }

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Perform the conversion from MBOX to PST
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
            {
                Console.WriteLine("MBOX to PST conversion completed successfully.");
            }
        }
        catch (Exception ex)
        {
            // Global exception handling
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
