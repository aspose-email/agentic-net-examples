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
            string mboxPath = "sample.mbox";
            string outputHtmlPath = "firstMessage.html";

            // Verify input MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {mboxPath}");
                return;
            }

            // Ensure the output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(outputHtmlPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Open the MBOX file stream
            try
            {
                using (FileStream fileStream = new FileStream(mboxPath, FileMode.Open, FileAccess.Read))
                {
                    MboxLoadOptions loadOptions = new MboxLoadOptions();

                    // Create the reader using the factory method
                    using (MboxStorageReader reader = MboxStorageReader.CreateReader(fileStream, loadOptions))
                    {
                        MailMessage message;
                        // Read messages sequentially
                        while ((message = reader.ReadNextMessage()) != null)
                        {
                            try
                            {
                                // Save the first message as HTML
                                HtmlSaveOptions htmlOptions = new HtmlSaveOptions();
                                message.Save(outputHtmlPath, htmlOptions);
                                Console.WriteLine($"First message saved to {outputHtmlPath}");
                            }
                            catch (Exception saveEx)
                            {
                                Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                            }
                            finally
                            {
                                // Dispose the message after processing
                                if (message != null)
                                    message.Dispose();
                            }

                            // Process only the first message for this example
                            break;
                        }
                    }
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
