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
            // Paths for the encrypted MBOX file and the output PST file.
            string mboxPath = "encrypted.mbox";
            string pstPath = "output.pst";

            // Verify that the input MBOX file exists.
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // NOTE: Aspose.Email does not expose a direct Password property on MboxLoadOptions.
            // If the MBOX file is encrypted, it must be decrypted before processing.
            // The example proceeds with conversion assuming the file is accessible.

            // Optional: configure load options (e.g., preferred encoding).
            MboxLoadOptions loadOptions = new MboxLoadOptions
            {
                PreferredTextEncoding = System.Text.Encoding.UTF8,
                LeaveOpen = false
            };

            // Create a reader for the MBOX file using the load options.
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                // Convert the MBOX storage to PST using the static converter.
                // The conversion method does not require explicit options for password handling.
                PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath);

                // Dispose the PST storage explicitly.
                pst.Dispose();

                Console.WriteLine($"Conversion completed successfully. PST saved to: {pstPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
