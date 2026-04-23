using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output directories
            string inputFolder = "InputEmails";
            string outputFolder = "UniqueEmails";

            // Verify input folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolder}");
                return;
            }

            // Ensure output folder exists
            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output folder: {dirEx.Message}");
                    return;
                }
            }

            // Track seen Message-Id values
            HashSet<string> seenMessageIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Get all .eml files in the input folder
            string[] emlFiles;
            try
            {
                emlFiles = Directory.GetFiles(inputFolder, "*.eml");
            }
            catch (Exception fileEx)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {fileEx.Message}");
                return;
            }

            foreach (string emlPath in emlFiles)
            {
                // Guard against missing file
                if (!File.Exists(emlPath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found: {emlPath}");
                    continue;
                }

                try
                {
                    // Load the email message
                    using (MailMessage message = MailMessage.Load(emlPath))
                    {
                        // Retrieve the Message-Id header
                        string messageId = message.Headers["Message-Id"];

                        // If Message-Id is missing, generate a temporary unique identifier
                        if (string.IsNullOrEmpty(messageId))
                        {
                            messageId = Guid.NewGuid().ToString();
                        }

                        // Skip duplicate messages
                        if (seenMessageIds.Contains(messageId))
                        {
                            Console.WriteLine($"Skipping duplicate message with Message-Id: {messageId}");
                            continue;
                        }

                        // Record the Message-Id as seen
                        seenMessageIds.Add(messageId);

                        // Prepare output file path
                        string outputFileName = Path.GetFileNameWithoutExtension(emlPath) + ".msg";
                        string outputPath = Path.Combine(outputFolder, outputFileName);

                        // Save the message as MSG format
                        MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);
                        message.Save(outputPath, saveOptions);
                    }
                }
                catch (Exception msgEx)
                {
                    Console.Error.WriteLine($"Error processing file '{emlPath}': {msgEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
