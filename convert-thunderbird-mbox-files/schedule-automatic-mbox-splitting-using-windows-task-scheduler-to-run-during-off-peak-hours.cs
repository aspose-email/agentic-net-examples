using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string outputFolder = "SplitMbox";

            // Verify the MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Create a reader with load options as required by validation rules
            MboxLoadOptions loadOptions = new MboxLoadOptions();
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                // Split the MBOX into parts of 10 MB each
                long partSize = 10 * 1024 * 1024; // 10 MB
                try
                {
                    reader.SplitInto(partSize, outputFolder);
                    Console.WriteLine($"MBOX split completed. Parts are stored in '{outputFolder}'.");
                }
                catch (Exception splitEx)
                {
                    Console.Error.WriteLine($"Error during splitting: {splitEx.Message}");
                }

                // Example of reading messages one by one using ReadNextMessage
                MailMessage message;
                while ((message = reader.ReadNextMessage()) != null)
                {
                    Console.WriteLine($"Read message: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
