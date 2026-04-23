using Aspose.Email;
using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        try
        {
            // Ensure the archive directory exists
            string archiveDir = "Archive";
            if (!Directory.Exists(archiveDir))
            {
                Directory.CreateDirectory(archiveDir);
            }

            // Create a simple email message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Sample Email";
            message.Body = "This is a sample email for archival.";

            // Save the raw MIME content to a memory stream
            using (MemoryStream mimeStream = new MemoryStream())
            {
                message.Save(mimeStream);
                mimeStream.Position = 0;
                byte[] mimeBytes = mimeStream.ToArray();

                // Asynchronously store the MIME content in a blob (simulated by a file)
                string blobPath = Path.Combine(archiveDir, "sample.eml");
                await UploadToBlobAsync(blobPath, mimeBytes);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Simulated async upload to a blob storage service
    private static async Task UploadToBlobAsync(string blobPath, byte[] data)
    {
        try
        {
            await File.WriteAllBytesAsync(blobPath, data);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Blob upload failed: {ex.Message}");
        }
    }
}
