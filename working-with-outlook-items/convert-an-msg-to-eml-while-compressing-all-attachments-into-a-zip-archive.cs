using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "input.msg";
            string outputEmlPath = "output.eml";
            string tempZipPath = "attachments.zip";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputEmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load MSG file
            using (MailMessage message = MailMessage.Load(inputMsgPath))
            {
                // If there are attachments, compress them into a zip archive
                if (message.Attachments.Count > 0)
                {
                    // Create zip archive containing all original attachments
                    using (FileStream zipFileStream = new FileStream(tempZipPath, FileMode.Create, FileAccess.Write))
                    using (ZipArchive zipArchive = new ZipArchive(zipFileStream, ZipArchiveMode.Create, leaveOpen: false))
                    {
                        foreach (Attachment attachment in message.Attachments)
                        {
                            // Use attachment name or a default name
                            string entryName = string.IsNullOrEmpty(attachment.Name) ? "attachment" : attachment.Name;
                            ZipArchiveEntry zipEntry = zipArchive.CreateEntry(entryName, CompressionLevel.Optimal);
                            using (Stream entryStream = zipEntry.Open())
                            using (Stream attachmentStream = attachment.ContentStream)
                            {
                                attachmentStream.CopyTo(entryStream);
                            }
                        }
                    }

                    // Remove original attachments
                    message.Attachments.Clear();

                    // Add the zip archive as a single attachment
                    Attachment zipAttachment = new Attachment(tempZipPath);
                    message.Attachments.Add(zipAttachment);
                }

                // Save the message as EML using default save options
                message.Save(outputEmlPath, SaveOptions.DefaultEml);
            }

            // Clean up temporary zip file
            if (File.Exists(tempZipPath))
            {
                try
                {
                    File.Delete(tempZipPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to delete temporary zip file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
