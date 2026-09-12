using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        // Paths for input MBOX (Thunderbird) and output PST files
        string mboxFilePath = "input.mbox";
        string pstFilePath = "output.pst";

        // Ensure the input MBOX file exists; create an empty placeholder if missing
        if (!File.Exists(mboxFilePath))
        {
            try
            {
                File.WriteAllText(mboxFilePath, string.Empty);
                Console.WriteLine($"Created placeholder MBOX file at '{mboxFilePath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                return;
            }
        }

        // Ensure the directory for the PST file exists
        string pstDirectory = Path.GetDirectoryName(pstFilePath);
        if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
        {
            try
            {
                Directory.CreateDirectory(pstDirectory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }
        }

        try
        {
            // Convert the MBOX storage to PST. The method returns a PersonalStorage instance.
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath))
            {
                // Optional: display basic information about the created PST
                int totalItems = pst.Store.GetTotalItemsCount();
                Console.WriteLine($"Conversion successful. PST contains {totalItems} total items.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Conversion failed: {ex.Message}");
        }
    }
}
