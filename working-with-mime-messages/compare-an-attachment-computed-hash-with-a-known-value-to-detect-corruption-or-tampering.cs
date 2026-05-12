using System;
using System.IO;
using System.Security.Cryptography;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AttachmentHashCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MSG file containing the attachment
                string msgPath = "sample.msg";

                // Verify that the file exists before attempting to load it
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

                    Console.Error.WriteLine($"Message file not found: {msgPath}");
                    return;
                }

                // Known SHA256 hash value to compare against (hexadecimal string)
                string knownHash = "ABCDEF1234567890ABCDEF1234567890ABCDEF1234567890ABCDEF1234567890";

                // Load the MSG file inside a using block to ensure proper disposal
                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    if (message.Attachments == null || message.Attachments.Count == 0)
                    {
                        Console.WriteLine("No attachments found in the message.");
                        return;
                    }

                    // Iterate through each attachment and compute its SHA256 hash
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        byte[] attachmentData = attachment.BinaryData;

                        if (attachmentData == null || attachmentData.Length == 0)
                        {
                            Console.WriteLine($"Attachment \"{attachment.FileName}\" is empty.");
                            continue;
                        }

                        // Compute SHA256 hash of the attachment data
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            byte[] hashBytes = sha256.ComputeHash(attachmentData);
                            string computedHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                            bool hashesMatch = string.Equals(computedHash, knownHash, StringComparison.OrdinalIgnoreCase);

                            Console.WriteLine($"Attachment: {attachment.FileName}");
                            Console.WriteLine($"Computed SHA256: {computedHash}");
                            Console.WriteLine(hashesMatch ? "Hash matches the known value." : "Hash does NOT match the known value.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
