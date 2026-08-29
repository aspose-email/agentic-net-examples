using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        const string inputMboxPath = "input.mbox";
        const string errorMboxPath = "error.mbox";

        // Verify input file exists
        if (!File.Exists(inputMboxPath))
        {
            Console.Error.WriteLine($"Input MBOX file not found: {inputMboxPath}");
            return;
        }

        // Ensure directory for error MBOX exists
        string? errorDir = Path.GetDirectoryName(errorMboxPath);
        if (!string.IsNullOrEmpty(errorDir) && !Directory.Exists(errorDir))
        {
            Directory.CreateDirectory(errorDir);
        }

        try
        {
            // Create reader with default load options
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(inputMboxPath, new MboxLoadOptions()))
            // Create writer (concrete implementation) for failed messages
            using (MboxrdStorageWriter errorWriter = new MboxrdStorageWriter(errorMboxPath, new MboxSaveOptions()))
            {
                while (true)
                {
                    // Read next message sequentially; returns null when end of file is reached
                    MailMessage message = reader.ReadNextMessage();
                    if (message == null)
                        break;

                    try
                    {
                        // Example processing: output basic info
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"To: {message.To}");
                        // Add additional processing logic here
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Processing failed for message '{message.Subject}': {ex.Message}");
                        // Write the original message to the error MBOX for later review
                        errorWriter.WriteMessage(message);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Unexpected error: {e.Message}");
        }
    }
}
