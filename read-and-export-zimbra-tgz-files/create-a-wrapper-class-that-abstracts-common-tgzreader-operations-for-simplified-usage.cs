using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

namespace TgzReaderSample
{
    // Wrapper class that simplifies common TgzReader operations
    public class TgzReaderWrapper : IDisposable
    {
        private TgzReader reader;

        // Initializes the wrapper with a TGZ file path
        public TgzReaderWrapper(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path is null or empty.", nameof(filePath));
            }

            this.reader = new TgzReader(filePath);
        }

        // Returns the total number of items in the TGZ archive
        public int GetTotalItemsCount()
        {
            return this.reader.GetTotalItemsCount();
        }

        // Reads the next message in the archive
        public void ReadNextMessage()
        {
            this.reader.ReadNextMessage();
        }

        // Retrieves the current message after a ReadNextMessage call
        public MailMessage GetCurrentMessage()
        {
            return this.reader.CurrentMessage;
        }

        // Exports all messages and folder structure to the specified directory
        public void ExportTo(string outputPath)
        {
            this.reader.ExportTo(outputPath);
        }

        // Disposes the underlying TgzReader
        public void Dispose()
        {
            this.reader?.Dispose();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the TGZ file (replace with actual path)
                string tgzFilePath = "sample.tgz";

                // Directory where extracted messages will be saved
                string exportDirectory = "ExportedMessages";

                // Guard input file existence
                if (!File.Exists(tgzFilePath))
                {
                    Console.Error.WriteLine($"Input file '{tgzFilePath}' does not exist. Skipping execution.");
                    return;
                }

                // Ensure output directory exists
                if (!Directory.Exists(exportDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(exportDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create output directory '{exportDirectory}': {dirEx.Message}");
                        return;
                    }
                }

                // Use the wrapper to work with the TGZ archive
                using (TgzReaderWrapper tgzWrapper = new TgzReaderWrapper(tgzFilePath))
                {
                    int totalItems = tgzWrapper.GetTotalItemsCount();
                    Console.WriteLine($"Total items in TGZ archive: {totalItems}");

                    // Export all messages to the output directory
                    tgzWrapper.ExportTo(exportDirectory);
                    Console.WriteLine($"All messages exported to '{exportDirectory}'.");

                    // Iterate through messages one by one
                    for (int i = 0; i < totalItems; i++)
                    {
                        tgzWrapper.ReadNextMessage();
                        MailMessage currentMessage = tgzWrapper.GetCurrentMessage();

                        if (currentMessage != null)
                        {
                            Console.WriteLine($"Message {i + 1}: Subject = {currentMessage.Subject}");
                            // Additional processing of currentMessage can be done here
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
