using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string uniqueId = "12345";
            string outputPath = "message.eml";

            // Skip execution if placeholder credentials are detected
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            // Ensure the output directory exists
            try
            {
                string directory = Path.GetDirectoryName(outputPath);
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

            // Create and use the IMAP client
            try
            {
                using (ImapClient client = new ImapClient(host, port, SecurityOptions.SSLImplicit))
                {
                    client.Username = username;
                    client.Password = password;

                    // Validate credentials with a lightweight operation
                    try
                    {
                        await client.SelectFolderAsync("INBOX");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to select folder: {ex.Message}");
                        return;
                    }

                    // Fetch the message by its unique identifier
                    MailMessage message;
                    try
                    {
                        message = await client.FetchMessageAsync(uniqueId);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch message: {ex.Message}");
                        return;
                    }

                    // Save the message as .eml
                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Client error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
