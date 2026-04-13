using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define input directory containing MSG files
            string inputDirectory = "InputMsgs";

            // Verify the input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Error: Input directory not found – {inputDirectory}");
                return;
            }

            // Define the output ZIP file path
            string zipPath = "ArchivedMessages.zip";

            // Create or overwrite the ZIP archive
            try
            {
                using (FileStream zipFileStream = new FileStream(zipPath, FileMode.Create, FileAccess.ReadWrite))
                using (ZipArchive zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Update))
                {
                    // Get all .msg files in the input directory
                    string[] msgFiles = Directory.GetFiles(inputDirectory, "*.msg", SearchOption.TopDirectoryOnly);

                    foreach (string msgFilePath in msgFiles)
                    {
                        // Guard against missing files (should not happen after GetFiles, but safe)
                        if (!File.Exists(msgFilePath))
                        {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                            Console.Error.WriteLine($"Warning: File not found – {msgFilePath}");
                            continue;
                        }

                        // Load the MSG file into a MapiMessage
                        MapiMessage message;
                        try
                        {
                            message = MapiMessage.Load(msgFilePath);
                        }
                        catch (Exception loadEx)
                        {
                            Console.Error.WriteLine($"Error loading MSG file '{msgFilePath}': {loadEx.Message}");
                            continue;
                        }

                        // Determine the message date (use ClientSubmitTime if available, otherwise file creation time)
                        DateTime messageDate = message.ClientSubmitTime != DateTime.MinValue
                            ? message.ClientSubmitTime
                            : File.GetCreationTime(msgFilePath);

                        // Check if the message is older than one year
                        if (messageDate < DateTime.Now.AddYears(-1))
                        {
                            // Add the original MSG file to the ZIP archive
                            try
                            {
                                string entryName = Path.GetFileName(msgFilePath);
                                ZipArchiveEntry zipEntry = zipArchive.CreateEntry(entryName, CompressionLevel.Optimal);
                                using (Stream entryStream = zipEntry.Open())
                                using (FileStream sourceStream = new FileStream(msgFilePath, FileMode.Open, FileAccess.Read))
                                {
                                    sourceStream.CopyTo(entryStream);
                                }
                            }
                            catch (Exception zipEx)
                            {
                                Console.Error.WriteLine($"Error adding file to ZIP '{msgFilePath}': {zipEx.Message}");
                            }
                        }

                        // Dispose the loaded message
                        message.Dispose();
                    }
                }
            }
            catch (Exception zipEx)
            {
                Console.Error.WriteLine($"Error creating ZIP archive: {zipEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
