using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            const string inputMboxPath = "input.mbox";
            const string outputPstPath = "output.pst";
            const int maxBodyLength = 1000;

            // Verify that the input MBOX file exists.
            if (!File.Exists(inputMboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {inputMboxPath}");
                return;
            }

            // Ensure the output directory exists.
            string outputDirectory = Path.GetDirectoryName(outputPstPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Handler that truncates message bodies exceeding the specified length.
            MailStorageConverter.MailHandler handler = delegate (MailMessage message)
            {
                if (message.Body != null && message.Body.Length > maxBodyLength)
                {
                    string truncated = message.Body.Substring(0, maxBodyLength) + "...";
                    message.Body = truncated;
                }
            };

            // Convert the MBOX to PST using the handler.
            try
            {
                MailStorageConverter.MboxToPst(inputMboxPath, outputPstPath, handler);
                Console.WriteLine("Conversion completed successfully.");
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
