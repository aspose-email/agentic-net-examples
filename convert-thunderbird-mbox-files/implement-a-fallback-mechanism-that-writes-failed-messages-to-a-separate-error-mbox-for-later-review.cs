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
            // Define file paths
            string inputMboxPath = "input.mbox";
            string outputPstPath = "output.pst";
            string errorMboxPath = "error.mbox";

            // Guard input file existence
            if (!File.Exists(inputMboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {inputMboxPath}");
                return;
            }

            // Ensure output directories exist
            string outputDir = Path.GetDirectoryName(outputPstPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            string errorDir = Path.GetDirectoryName(errorMboxPath);
            if (!string.IsNullOrEmpty(errorDir) && !Directory.Exists(errorDir))
            {
                Directory.CreateDirectory(errorDir);
            }

            // Create writer for error MBOX
            using (MboxrdStorageWriter errorWriter = new MboxrdStorageWriter(errorMboxPath, new MboxSaveOptions()))
            {
                // Define handler that processes each message and writes failures to error MBOX
                MailStorageConverter.MailHandler handler = (MailMessage msg) =>
                {
                    try
                    {
                        // Placeholder processing logic (e.g., log subject)
                        Console.WriteLine($"Processing message: {msg.Subject}");
                    }
                    catch (Exception processingEx)
                    {
                        // On processing failure, write the original message to the error MBOX
                        try
                        {
                            errorWriter.WriteMessage(msg);
                        }
                        catch (Exception writeEx)
                        {
                            Console.Error.WriteLine($"Failed to write to error MBOX: {writeEx.Message}");
                        }
                    }
                };

                // Perform conversion with the custom handler
                try
                {
                    MailStorageConverter.MboxToPst(inputMboxPath, outputPstPath, handler);
                }
                catch (Aspose.Email.AsposeException convEx)
                {
                    Console.Error.WriteLine($"Conversion failed: {convEx.Message}");
                }
                catch (Exception convEx)
                {
                    Console.Error.WriteLine($"Unexpected error during conversion: {convEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
