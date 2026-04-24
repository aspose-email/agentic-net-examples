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
        try
        {
            const string mboxPath = "input.mbox";
            const string pstPath = "output.pst";
            const long maxMboxSizeBytes = 100L * 1024L * 1024L; // 100 MB

            // Verify MBOX file existence
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Check file size against limit
            FileInfo mboxInfo = new FileInfo(mboxPath);
            if (mboxInfo.Length > maxMboxSizeBytes)
            {
                Console.Error.WriteLine($"MBOX file size ({mboxInfo.Length} bytes) exceeds the allowed limit of {maxMboxSizeBytes} bytes.");
                return;
            }

            // Perform conversion inside a safe block
            try
            {
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
                {
                    // Conversion succeeded; optionally inform the user
                    Console.WriteLine($"MBOX file '{mboxPath}' successfully converted to PST file '{pstPath}'.");
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
