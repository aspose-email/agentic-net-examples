using Aspose.Email.Storage.Pst;
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
            // Input MBOX file path
            string mboxPath = "input.mbox";
            // Output PST file path
            string pstPath = "output.pst";

            // Verify input file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Configure conversion options
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();

            // The current SDK version does not expose a direct CategoryFolderMap property.
            // If such a mapping API exists, set it here, e.g.:
            // options.CategoryFolderMap.Add("Work", "Inbox\\Work");
            // options.CategoryFolderMap.Add("Personal", "Inbox\\Personal");
            // Otherwise, adjust the options according to the available members.

            // Perform the conversion and obtain the PST storage object
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options))
            {
                // Conversion completed; the PST file is saved at pstPath.
            }

            Console.WriteLine("MBOX to PST conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

// Author note: This sample shows how to configure MboxToPstConversionOptions for custom category-to-folder mapping.
// Replace the placeholder mapping code with actual API members if they are available in your Aspose.Email version.
