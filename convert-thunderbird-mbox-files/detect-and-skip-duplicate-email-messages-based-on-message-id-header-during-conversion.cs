using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

namespace AsposeEmailDuplicateSkip
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input MBOX file path
                string inputMboxPath = "storage.mbox";
                // Output directory for extracted messages
                string outputDirectory = "output";

                // Verify input file exists
                if (!File.Exists(inputMboxPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputMboxPath}");
                    return;
                }

                // Ensure output directory exists
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }

                // Set to track processed Message-Id headers
                HashSet<string> processedMessageIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                // Create MboxStorageReader
                MboxStorageReader mboxReader = MboxStorageReader.CreateReader(inputMboxPath, new MboxLoadOptions());

                // Iterate through each message in the MBOX storage
                foreach (MboxMessageInfo mboxMessageInfo in mboxReader.EnumerateMessageInfo())
                {
                    try
                    {
                        // Extract the full MIME message
                        using (MailMessage mailMessage = mboxReader.ExtractMessage(mboxMessageInfo.EntryId, new EmlLoadOptions()))
                        {
                            // Retrieve the Message-Id header
                            string messageId = mailMessage.Headers["Message-Id"];

                            // If Message-Id is missing, treat as unique (or could generate a fallback)
                            if (!string.IsNullOrEmpty(messageId))
                            {
                                if (processedMessageIds.Contains(messageId))
                                {
                                    // Duplicate detected – skip saving
                                    Console.WriteLine($"Skipping duplicate message with Message-Id: {messageId}");
                                    continue;
                                }
                                processedMessageIds.Add(messageId);
                            }

                            // Build a safe file name using the subject (fallback to GUID)
                            string safeSubject = string.IsNullOrWhiteSpace(mailMessage.Subject) ? Guid.NewGuid().ToString() : mailMessage.Subject;
                            foreach (char invalidChar in Path.GetInvalidFileNameChars())
                            {
                                safeSubject = safeSubject.Replace(invalidChar, '_');
                            }
                            string outputPath = Path.Combine(outputDirectory, $"{safeSubject}.eml");

                            // Save the message
                            mailMessage.Save(outputPath);
                            Console.WriteLine($"Saved message: {outputPath}");
                        }
                    }
                    catch (Exception msgEx)
                    {
                        Console.Error.WriteLine($"Error processing message ID {mboxMessageInfo.EntryId}: {msgEx.Message}");
                        // Continue with next message
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
