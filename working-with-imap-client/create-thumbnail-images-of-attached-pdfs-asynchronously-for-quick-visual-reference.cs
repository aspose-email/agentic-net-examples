using Aspose.Email.Clients;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string folderName = "INBOX";
            string outputDir = "Attachments";

            // Skip real network calls when placeholders are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Connect to IMAP server
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Select the target folder
                    await client.SelectFolderAsync(folderName, CancellationToken.None);

                    // Retrieve message infos
                    ImapMessageInfoCollection messages = await client.ListMessagesAsync(folderName, false, CancellationToken.None);

                    foreach (ImapMessageInfo info in messages)
                    {
                        // Fetch the full message
                        MailMessage message = await client.FetchMessageAsync(info.UniqueId, CancellationToken.None);

                        // Get PDF attachments
                        var pdfAttachments = message.Attachments
                            .Where(a => a.Name != null && a.Name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                            .ToList();

                        foreach (Attachment attachment in pdfAttachments)
                        {
                            // Save PDF to disk
                            string pdfPath = Path.Combine(outputDir, attachment.Name);
                            try
                            {
                                using (FileStream pdfStream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write))
                                {
                                    attachment.ContentStream.CopyTo(pdfStream);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save PDF '{attachment.Name}': {ex.Message}");
                                continue;
                            }

                            // Placeholder thumbnail generation
                            // In a real scenario, use a PDF rendering library to create an image.
                            string thumbPath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(attachment.Name) + "_thumb.png");
                            try
                            {
                                // Create a minimal placeholder PNG file (empty content)
                                using (FileStream thumbStream = new FileStream(thumbPath, FileMode.Create, FileAccess.Write))
                                {
                                    // PNG header for an empty 1x1 pixel image
                                    byte[] pngHeader = new byte[]
                                    {
                                        0x89,0x50,0x4E,0x47,0x0D,0x0A,0x1A,0x0A,
                                        0x00,0x00,0x00,0x0D,0x49,0x48,0x44,0x52,
                                        0x00,0x00,0x00,0x01,0x00,0x00,0x00,0x01,
                                        0x08,0x06,0x00,0x00,0x00,0x1F,0x15,0xC4,
                                        0x89,0x00,0x00,0x00,0x0A,0x49,0x44,0x41,
                                        0x54,0x78,0x9C,0x63,0x00,0x01,0x00,0x00,
                                        0x05,0x00,0x01,0x0D,0x0A,0x2D,0xB4,0x00,
                                        0x00,0x00,0x00,0x49,0x45,0x4E,0x44,0xAE,
                                        0x42,0x60,0x82
                                    };
                                    thumbStream.Write(pngHeader, 0, pngHeader.Length);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to create thumbnail for '{attachment.Name}': {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
