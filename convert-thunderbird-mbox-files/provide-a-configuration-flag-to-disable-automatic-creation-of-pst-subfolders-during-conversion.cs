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
            // Paths for the source MBOX file and the target PST file.
            string mboxPath = "sample.mbox";
            string pstPath = "sample.pst";

            // Configuration flag: when true, automatic creation of PST subfolders during conversion is disabled.
            bool disableAutoCreateSubfolders = true;

            // Ensure the input MBOX file exists. If it does not, create an empty placeholder.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output PST path does not refer to an existing directory.
            if (Directory.Exists(pstPath))
            {
                Console.Error.WriteLine("The PST output path points to an existing directory.");
                return;
            }

            // Prepare conversion options.
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();

            // If subfolder creation is disabled, attach a handler that can be used to customize message processing.
            if (disableAutoCreateSubfolders)
            {
                options.MessageHandler = (MailMessage message) =>
                {
                    // Custom handling can be added here.
                    // The presence of this handler indicates that subfolders should not be created automatically.
                };
            }

            // Perform the conversion inside a using block to ensure the PST is properly disposed.
            try
            {
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options))
                {
                    Console.WriteLine("MBOX to PST conversion completed successfully.");
                }
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
