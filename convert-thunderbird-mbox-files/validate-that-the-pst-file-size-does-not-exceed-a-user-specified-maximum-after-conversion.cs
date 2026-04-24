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
            // Paths for source MBOX and target PST
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Verify the source MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Prompt user for maximum allowed PST size (in megabytes)
            Console.Write("Enter maximum PST size in megabytes: ");
            string sizeInput = Console.ReadLine();
            if (!double.TryParse(sizeInput, out double maxMegabytes) || maxMegabytes <= 0)
            {
                Console.Error.WriteLine("Invalid size value.");
                return;
            }
            long maxBytes = (long)(maxMegabytes * 1024 * 1024);

            // Convert MBOX to PST
            PersonalStorage pst = null;
            try
            {
                pst = MailStorageConverter.MboxToPst(mboxPath, pstPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }
            finally
            {
                pst?.Dispose();
            }

            // Verify the resulting PST file size
            try
            {
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"PST file was not created: {pstPath}");
                    return;
                }

                FileInfo pstInfo = new FileInfo(pstPath);
                long pstSize = pstInfo.Length;
                Console.WriteLine($"PST size: {pstSize} bytes");

                if (pstSize > maxBytes)
                {
                    Console.WriteLine("PST exceeds the maximum allowed size.");
                }
                else
                {
                    Console.WriteLine("PST size is within the allowed limit.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error checking PST size: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
