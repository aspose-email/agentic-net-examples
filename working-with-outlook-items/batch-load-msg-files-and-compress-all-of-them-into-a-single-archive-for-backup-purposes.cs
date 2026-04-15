using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputFolder = "MsgFiles";
            string outputZip = "BackupArchive.zip";

            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Error: Input folder not found – {inputFolder}");
                return;
            }

            string[] msgFiles = Directory.GetFiles(inputFolder, "*.msg");
            if (msgFiles.Length == 0)
            {
                Console.Error.WriteLine("No MSG files found to backup.");
                return;
            }

            // Create or overwrite the zip archive
            using (FileStream zipStream = new FileStream(outputZip, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Update))
            {
                foreach (string msgPath in msgFiles)
                {
                    try
                    {
                        if (!File.Exists(msgPath))
                        {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                            Console.Error.WriteLine($"Skipping missing file: {msgPath}");
                            continue;
                        }

                        // Load the MSG file into a MailMessage
                        using (MailMessage message = MailMessage.Load(msgPath))
                        {
                            // Save the message to a memory stream in MSG format
                            using (MemoryStream msgStream = new MemoryStream())
                            {
                                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                                message.Save(msgStream, saveOptions);
                                msgStream.Position = 0;

                                // Add the MSG stream as an entry in the zip archive
                                string entryName = Path.GetFileName(msgPath);
                                ZipArchiveEntry entry = archive.CreateEntry(entryName, CompressionLevel.Optimal);
                                using (Stream entryStream = entry.Open())
                                {
                                    msgStream.CopyTo(entryStream);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing file {msgPath}: {ex.Message}");
                    }
                }
            }

            Console.WriteLine($"Backup completed: {outputZip}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
