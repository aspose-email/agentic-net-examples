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
            // Define input MBOX file and output PST file paths
            string mboxFilePath = "input.mbox";
            string pstFilePath = "output.pst";

            // Verify that the input MBOX file exists
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxFilePath}");
                return;
            }

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory for PST file: {dirEx.Message}");
                    return;
                }
            }

            // Configure conversion options (example: remove signatures)
            MboxToPstConversionOptions conversionOptions = new MboxToPstConversionOptions
            {
                RemoveSignature = false
            };

            // Open the MBOX file for reading using a reader
            using (FileStream mboxStream = File.OpenRead(mboxFilePath))
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxStream, new MboxLoadOptions()))
            // Create a new PST file (Unicode format) for writing
            using (PersonalStorage pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
            {
                // Convert all messages from the MBOX reader into the PST under the "Inbox" folder
                MailStorageConverter.MboxToPst(mboxReader, pst, "Inbox", conversionOptions);
                Console.WriteLine("MBOX to PST conversion completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
