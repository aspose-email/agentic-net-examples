using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Sequence number of the message to download.
            int messageSequenceNumber = 1;

            // Destination file for the streamed email.
            string outputFilePath = "message.eml";

            // Skip execution when placeholder credentials are detected.
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Ensure the output directory exists.
            try
            {
                string directory = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Connect to the IMAP server.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    client.SelectFolder("INBOX");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to select folder: {ex.Message}");
                    return;
                }

                // Stream the message directly to a file without loading it fully into memory.
                using (FileStream fileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    try
                    {
                        await client.SaveMessageAsync(messageSequenceNumber, fileStream);
                        Console.WriteLine($"Message saved to {outputFilePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
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
