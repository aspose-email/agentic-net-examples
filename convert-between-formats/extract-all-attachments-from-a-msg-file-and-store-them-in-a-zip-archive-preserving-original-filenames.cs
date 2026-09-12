using Aspose.Email;
using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Author note: This example extracts all attachments from a MSG file and stores them in a ZIP archive.
            string msgFilePath = "input.msg";
            string zipFilePath = "attachments.zip";

            // Verify input MSG file exists
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

                Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                return;
            }

            // Ensure the directory for the ZIP file exists
            string zipDirectory = Path.GetDirectoryName(zipFilePath);
            if (!string.IsNullOrEmpty(zipDirectory) && !Directory.Exists(zipDirectory))
            {
                try
                {
                    Directory.CreateDirectory(zipDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{zipDirectory}': {dirEx.Message}");
                    return;
                }
            }

            // Load the Outlook MSG file
            MapiMessage msg = MapiMessage.Load(msgFilePath);

            // Create the ZIP archive and add each attachment
            using (FileStream zipStream = new FileStream(zipFilePath, FileMode.Create, FileAccess.Write))
            using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: false))
            {
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    string attachmentFileName = attachment.FileName;
                    if (string.IsNullOrEmpty(attachmentFileName))
                    {
                        // Fallback to a generic name if the attachment has no filename
                        attachmentFileName = "unnamed_attachment";
                    }

                    // Save attachment to a memory stream
                    using (MemoryStream attachmentStream = new MemoryStream())
                    {
                        attachment.Save(attachmentStream);
                        attachmentStream.Position = 0;

                        // Create a new entry in the ZIP archive
                        ZipArchiveEntry zipEntry = zipArchive.CreateEntry(attachmentFileName, CompressionLevel.Optimal);
                        using (Stream entryStream = zipEntry.Open())
                        {
                            attachmentStream.CopyTo(entryStream);
                        }
                    }

                    Console.WriteLine($"Added attachment: {attachmentFileName}");
                }
            }

            Console.WriteLine($"All attachments have been saved to '{zipFilePath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
