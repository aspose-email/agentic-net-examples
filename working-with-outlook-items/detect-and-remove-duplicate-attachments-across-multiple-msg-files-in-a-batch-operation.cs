using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // List of MSG files to process
            string[] msgFiles = new string[]
            {
                @"C:\Emails\msg1.msg",
                @"C:\Emails\msg2.msg",
                @"C:\Emails\msg3.msg"
            };

            // Keep track of attachment hashes that have already been seen
            HashSet<string> seenAttachmentHashes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string msgPath in msgFiles)
            {
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

                    Console.Error.WriteLine($"File not found: {msgPath}");
                    continue;
                }

                try
                {
                    using (MapiMessage message = MapiMessage.Load(msgPath))
                    {
                        // Collect attachments to remove after iteration to avoid modifying collection while enumerating
                        List<MapiAttachment> attachmentsToRemove = new List<MapiAttachment>();

                        foreach (MapiAttachment attachment in message.Attachments)
                        {
                            byte[] attachmentData;

                            // Try to get raw bytes of the attachment
                            using (MemoryStream ms = new MemoryStream())
                            {
                                attachment.Save(ms);
                                attachmentData = ms.ToArray();
                            }

                            // Compute SHA256 hash of the attachment content
                            using (SHA256 sha256 = SHA256.Create())
                            {
                                byte[] hashBytes = sha256.ComputeHash(attachmentData);
                                string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                                if (seenAttachmentHashes.Contains(hashString))
                                {
                                    // Duplicate found – mark for removal
                                    attachmentsToRemove.Add(attachment);
                                }
                                else
                                {
                                    // First occurrence – remember the hash
                                    seenAttachmentHashes.Add(hashString);
                                }
                            }
                        }

                        // Remove duplicate attachments
                        foreach (MapiAttachment dupAttachment in attachmentsToRemove)
                        {
                            message.Attachments.Remove(dupAttachment);
                        }

                        // Save the modified message back to the same file
                        message.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{msgPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
