using Aspose.Email.Clients.Exchange.Dav;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // Create Exchange client using the required 'using' pattern
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // List of attachment URIs representing the split parts (example placeholders)
                List<string> attachmentUris = new List<string>
                {
                    "attachmentUriPart1",
                    "attachmentUriPart2",
                    "attachmentUriPart3"
                };

                // Collect byte arrays of each part
                List<byte[]> partBytes = new List<byte[]>();

                foreach (string uri in attachmentUris)
                {
                    try
                    {
                        Attachment attachment = client.FetchAttachment(uri);
                        if (attachment == null || attachment.ContentStream == null)
                        {
                            Console.Error.WriteLine($"Attachment not found or empty for URI: {uri}");
                            continue;
                        }

                        using (MemoryStream memory = new MemoryStream())
                        {
                            attachment.ContentStream.CopyTo(memory);
                            partBytes.Add(memory.ToArray());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error fetching attachment '{uri}': {ex.Message}");
                        // Continue with remaining parts
                    }
                }

                if (partBytes.Count == 0)
                {
                    Console.Error.WriteLine("No attachment parts were retrieved. Aborting reassembly.");
                    return;
                }

                // Destination file path for the reassembled content
                string outputPath = "ReassembledFile.bin";

                // Ensure the output directory exists
                try
                {
                    string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                    return;
                }

                // Write all parts sequentially to recreate the original file
                try
                {
                    using (FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        foreach (byte[] bytes in partBytes)
                        {
                            outputStream.Write(bytes, 0, bytes.Length);
                        }
                    }

                    Console.WriteLine($"Reassembled file saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write reassembled file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
