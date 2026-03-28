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
            // Paths for source PST (Thunderbird export) and target MBOX (Zimbra import)
            string pstFilePath = "source.pst";
            string mboxFilePath = "target.mbox";

            // Verify source PST exists; if not, exit gracefully
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"Input PST file not found: {pstFilePath}");
                return;
            }

            // Ensure the directory for the MBOX file exists
            string mboxDirectory = Path.GetDirectoryName(mboxFilePath);
            if (!string.IsNullOrEmpty(mboxDirectory) && !Directory.Exists(mboxDirectory))
            {
                try
                {
                    Directory.CreateDirectory(mboxDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory for MBOX file: {ex.Message}");
                    return;
                }
            }

            // Convert PST to MBOX using Aspose.Email's MailboxConverter
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    // The third parameter is a MessageAcceptanceCallback; null means accept all messages
                    MailboxConverter.ConvertPersonalStorageToMbox(pst, mboxFilePath, null);
                }

                Console.WriteLine($"Conversion completed successfully. MBOX saved to: {mboxFilePath}");
            }
            catch (Exception convEx)
            {
                Console.Error.WriteLine($"Conversion failed: {convEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
