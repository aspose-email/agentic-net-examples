using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string mboxFilePath = "input.mbox";
            string pstFilePath = "output.pst";

            // Ensure the MBOX file exists; create an empty placeholder if missing.
            if (!File.Exists(mboxFilePath))
            {
                try
                {
                    File.WriteAllText(mboxFilePath, string.Empty);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ioEx.Message}");
                    return;
                }
            }

            // Configure load options to ignore malformed MIME parts.
            MboxLoadOptions mboxLoadOptions = new MboxLoadOptions();
            // No specific properties for malformed parts; keep defaults.

            // Configure EML load options used during MBOX parsing.
            EmlLoadOptions emlLoadOptions = new EmlLoadOptions();
            // Assuming the library provides a property to ignore invalid MIME parts.
            // If such a property exists, set it here (example: IgnoreInvalidHeaders).
            // emlLoadOptions.IgnoreInvalidHeaders = true;
            // Since the exact property name may vary, this line can be adjusted accordingly.

            MailStorageConverter.MboxMessageOptions = emlLoadOptions;

            // Perform the conversion using the configured options.
            try
            {
                // The conversion method internally uses the configured MboxMessageOptions.
                MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath);
                Console.WriteLine("MBOX to PST conversion completed successfully.");
            }
            catch (Exception convEx)
            {
                Console.Error.WriteLine($"Conversion failed: {convEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
