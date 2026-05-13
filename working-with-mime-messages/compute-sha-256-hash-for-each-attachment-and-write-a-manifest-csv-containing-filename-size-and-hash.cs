using System;
using System.IO;
using System.Security.Cryptography;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "message.msg";
            string csvPath = "attachment_manifest.csv";

            // Guard input file existence
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Ensure output directory exists
            string csvDir = Path.GetDirectoryName(csvPath);
            if (!string.IsNullOrEmpty(csvDir) && !Directory.Exists(csvDir))
            {
                try
                {
                    Directory.CreateDirectory(csvDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Open CSV writer
                using (StreamWriter writer = new StreamWriter(csvPath, false))
                {
                    // Write CSV header
                    writer.WriteLine("Filename,Size,SHA256Hash");

                    // Process each attachment
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        // Read attachment content into memory
                        using (MemoryStream ms = new MemoryStream())
                        {
                            attachment.Save(ms);
                            byte[] data = ms.ToArray();

                            // Compute SHA-256 hash
                            byte[] hashBytes;
                            using (SHA256 sha256 = SHA256.Create())
                            {
                                hashBytes = sha256.ComputeHash(data);
                            }
                            string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLowerInvariant();

                            // Write CSV line
                            string fileName = attachment.FileName ?? "Unnamed";
                            long size = data.Length;
                            writer.WriteLine($"{fileName},{size},{hashString}");
                        }
                    }
                }
            }

            Console.WriteLine($"Attachment manifest written to: {csvPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
