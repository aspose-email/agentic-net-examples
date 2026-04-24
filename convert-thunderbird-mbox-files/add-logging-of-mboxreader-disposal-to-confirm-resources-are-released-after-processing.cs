using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string mboxPath = "sample.mbox";
            string pstPath = "output.pst";

            // Ensure the MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ioEx.Message}");
                    return;
                }
            }

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create PST directory: {dirEx.Message}");
                    return;
                }
            }

            // Create MBOX reader with default load options
            MboxLoadOptions loadOptions = new MboxLoadOptions();
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                int messageCount = 0;
                foreach (var messageInfo in mboxReader.EnumerateMessageInfo())
                {
                    messageCount++;
                    // Example of extracting a message (optional)
                    // MailMessage message = mboxReader.ExtractMessage(messageInfo.EntryId, new EmlLoadOptions());
                    // message.Dispose();
                }
                Console.WriteLine($"Processed {messageCount} messages from MBOX.");
            }

            // Log disposal of the reader
            Console.WriteLine("MboxStorageReader disposed.");

            // Convert MBOX to PST using the static converter
            PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxPath, pstPath);
            pstStorage.Dispose();
            Console.WriteLine("PST file created successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
