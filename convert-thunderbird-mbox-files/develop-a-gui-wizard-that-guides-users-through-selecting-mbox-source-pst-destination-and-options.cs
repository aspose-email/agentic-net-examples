using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("=== MBOX to PST Conversion Wizard ===");

            // Prompt for source MBOX file
            Console.Write("Enter the full path to the source MBOX file: ");
            string mboxPath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(mboxPath) || !File.Exists(mboxPath))
            {
                Console.Error.WriteLine("Error: The specified MBOX file does not exist.");
                return;
            }

            // Prompt for destination PST file
            Console.Write("Enter the full path for the destination PST file: ");
            string pstPath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(pstPath))
            {
                Console.Error.WriteLine("Error: Invalid PST file path.");
                return;
            }

            // Ensure the destination directory exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error creating directory '{pstDirectory}': {dirEx.Message}");
                    return;
                }
            }

            // Prompt for optional conversion settings
            Console.Write("Preserve folder hierarchy from MBOX? (y/n): ");
            string preserveInput = Console.ReadLine();
            bool preserveFolders = preserveInput?.Trim().ToLower() == "y";

            // Prepare conversion options (default options are used; additional settings can be applied here)
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();
            // Note: Specific option properties (e.g., PreserveFolderStructure) can be set if available in the referenced version.

            // Perform the conversion
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options))
            {
                // The conversion method returns a PersonalStorage instance representing the created PST.
                // No further action is required for a basic conversion.
                Console.WriteLine("Conversion completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
