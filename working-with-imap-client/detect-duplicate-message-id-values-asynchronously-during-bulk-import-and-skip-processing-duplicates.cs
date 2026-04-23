using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Configuration (replace with real values)
            string host = "imap.example.com";
            int port = 993;
            string username = "user@example.com";
            string password = "password";
            string importFolder = "ImportFolder"; // IMAP folder to import into
            string sourceDirectory = "Messages";   // Local directory containing .eml files

            // Skip external calls when placeholders are used
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping IMAP operations.");
                return;
            }

            // Verify source directory exists
            if (!Directory.Exists(sourceDirectory))
            {
                Console.Error.WriteLine($"Source directory '{sourceDirectory}' does not exist.");
                return;
            }

            // Collect .eml files
            string[] emlFiles;
            try
            {
                emlFiles = Directory.GetFiles(sourceDirectory, "*.eml");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                return;
            }

            // Track processed Message-Id values
            HashSet<string> processedIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Create and connect IMAP client
            using (ImapClient client = new ImapClient(host, port, SecurityOptions.Auto))
            {
                try
                {
                    client.Username = username;
                    client.Password = password;
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"IMAP connection failed: {ex.Message}");
                    return;
                }

                // Ensure target folder exists
                try
                {
                    if (!client.ExistFolder(importFolder))
                    {
                        client.CreateFolder(importFolder);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to prepare folder '{importFolder}': {ex.Message}");
                    return;
                }

                // Process each .eml file
                foreach (string filePath in emlFiles)
                {
                    if (!File.Exists(filePath))
                    {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                        Console.Error.WriteLine($"File not found: {filePath}");
                        continue;
                    }

                    MailMessage message;
                    try
                    {
                        message = MailMessage.Load(filePath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to load message '{filePath}': {ex.Message}");
                        continue;
                    }

                    // Extract Message-Id header
                    string messageId = message.Headers["Message-Id"];
                    if (string.IsNullOrEmpty(messageId))
                    {
                        Console.Error.WriteLine($"Message-Id missing in '{filePath}'. Skipping.");
                        continue;
                    }

                    // Skip duplicates
                    if (processedIds.Contains(messageId))
                    {
                        Console.WriteLine($"Duplicate Message-Id detected: {messageId}. Skipping import.");
                        continue;
                    }

                    // Append message asynchronously
                    try
                    {
                        await client.AppendMessageAsync(importFolder, message);
                        processedIds.Add(messageId);
                        Console.WriteLine($"Imported message '{Path.GetFileName(filePath)}' with Message-Id: {messageId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to append message '{filePath}': {ex.Message}");
                    }
                    finally
                    {
                        message.Dispose();
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
