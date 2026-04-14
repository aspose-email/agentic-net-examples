using Aspose.Email;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Mapi;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string msgUrl = "https://example.com/message.msg"; // placeholder URL
            string localMsgPath = "message.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(localMsgPath))
            {
                try
                {
                    var placeholder = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Sample Subject",
                        "This is a placeholder message body.");
                    placeholder.Save(localMsgPath);
                    Console.WriteLine($"Created placeholder MSG file at '{localMsgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG asynchronously from a stream.
            MapiMessage message = await LoadMsgFromStreamAsync(localMsgPath);
            if (message != null)
            {
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"From: {message.SenderEmailAddress}");
                Console.WriteLine($"Body: {message.Body}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Asynchronously loads a MSG file from a stream.
    private static async Task<MapiMessage> LoadMsgFromStreamAsync(string filePath)
    {
        // Guard against missing file.
        if (!File.Exists(filePath))
        {
            Console.Error.WriteLine($"File '{filePath}' does not exist.");
            return null;
        }

        // Open the file stream asynchronously.
        try
        {
            using (FileStream stream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,
                useAsync: true))
            {
                // MapiMessage.Load is synchronous; wrap it in Task.Run to avoid blocking.
                return await Task.Run(() => MapiMessage.Load(stream));
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error loading MSG from stream: {ex.Message}");
            return null;
        }
    }
}
