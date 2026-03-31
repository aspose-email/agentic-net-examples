using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string mboxPath = "sample.mbox";
            string outputHtmlPath = "output.html";

            // Verify input MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Error: File not found – {mboxPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputHtmlPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                    return;
                }
            }

            // Create MBOX reader using the factory method
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                // Read the next (first) message sequentially
                MailMessage message = mboxReader.ReadNextMessage();
                if (message == null)
                {
                    Console.Error.WriteLine("No messages found in the MBOX file.");
                    return;
                }

                using (message)
                {
                    // Save the message as HTML
                    HtmlSaveOptions saveOptions = new HtmlSaveOptions();
                    try
                    {
                        message.Save(outputHtmlPath, saveOptions);
                        Console.WriteLine($"Message saved to {outputHtmlPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving HTML: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
