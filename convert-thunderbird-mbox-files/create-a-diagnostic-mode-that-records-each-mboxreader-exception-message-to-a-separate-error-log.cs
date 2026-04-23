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
            // Paths (adjust as needed)
            string mboxPath = "input.mbox";
            string outputFolder = "output";
            string errorLogPath = "error.log";

            // Guard input file
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                Directory.CreateDirectory(outputFolder);
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Open error log for appending
            using (StreamWriter errorLog = new StreamWriter(errorLogPath, true))
            {
                // Create the MBOX reader
                using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    int messageIndex = 0;
                    while (true)
                    {
                        MailMessage message = null;
                        try
                        {
                            // Read the next message; returns null when no more messages are available
                            message = reader.ReadNextMessage();
                            if (message == null)
                                break;

                            // Save each message as HTML
                            string htmlPath = Path.Combine(outputFolder, $"Message_{messageIndex}.html");
                            message.Save(htmlPath, new HtmlSaveOptions());

                            messageIndex++;
                        }
                        catch (Exception readEx)
                        {
                            // Record the exception for this message
                            errorLog.WriteLine($"{DateTime.Now:u} - Message {messageIndex}: {readEx.Message}");
                            // Continue with next message
                        }
                        finally
                        {
                            // Dispose the message if it was created
                            if (message != null)
                                message.Dispose();
                        }
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
