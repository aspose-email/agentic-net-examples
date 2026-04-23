using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // Define input MBOX and output PST paths
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Ensure the MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (FileStream fs = File.Create(mboxPath))
                    {
                        // Create an empty MBOX file
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for PST exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
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

            // Configure MBOX loading options: disable auto charset detection and enforce ISO‑8859‑1
            MboxLoadOptions loadOptions = new MboxLoadOptions
            {
                PreferredTextEncoding = Encoding.GetEncoding("ISO-8859-1")
            };

            // Create the MBOX reader
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                // Create a new PST file (Unicode format)
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Convert MBOX to PST, placing messages into the root folder named "Inbox"
                    MboxToPstConversionOptions conversionOptions = new MboxToPstConversionOptions();
                    MailStorageConverter.MboxToPst(mboxReader, pst, "Inbox", conversionOptions);
                }
            }

            Console.WriteLine("MBOX to PST conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
