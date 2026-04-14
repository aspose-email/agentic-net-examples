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
            // Input directory containing MSG files
            string inputDirectory = "InputMsgs";
            // Output directory for extracted unique attachments
            string outputDirectory = "OutputAttachments";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Error: Input directory not found – {inputDirectory}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Dictionary to track attachment hashes (to identify duplicates)
            Dictionary<string, string> attachmentHashMap = new Dictionary<string, string>();

            // Process each MSG file in the input directory
            foreach (string msgFilePath in Directory.GetFiles(inputDirectory, "*.msg"))
            {
                // Verify the MSG file exists before loading
                if (!File.Exists(msgFilePath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Warning: File not found – {msgFilePath}");
                    continue;
                }

                // Load the MSG file
                using (MapiMessage message = MapiMessage.Load(msgFilePath))
                {
                    // Iterate through each attachment in the message
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        // Compute SHA256 hash of the attachment content
                        string attachmentHash;
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            attachment.Save(memoryStream);
                            byte[] attachmentBytes = memoryStream.ToArray();
                            using (SHA256 sha256 = SHA256.Create())
                            {
                                byte[] hashBytes = sha256.ComputeHash(attachmentBytes);
                                attachmentHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                            }
                        }

                        // If this attachment hash has not been seen before, save it
                        if (!attachmentHashMap.ContainsKey(attachmentHash))
                        {
                            string safeFileName = Path.GetFileName(attachment.FileName);
                            string destinationPath = Path.Combine(outputDirectory, safeFileName);

                            // Ensure we do not overwrite an existing file with the same name
                            int duplicateCounter = 1;
                            while (File.Exists(destinationPath))
                            {
                                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(safeFileName);
                                string extension = Path.GetExtension(safeFileName);
                                string newFileName = $"{fileNameWithoutExt}_{duplicateCounter}{extension}";
                                destinationPath = Path.Combine(outputDirectory, newFileName);
                                duplicateCounter++;
                            }

                            // Save the attachment to the output directory
                            using (FileStream fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                            {
                                attachment.Save(fileStream);
                            }

                            // Record the hash to prevent future duplicates
                            attachmentHashMap.Add(attachmentHash, destinationPath);
                            Console.WriteLine($"Saved unique attachment: {destinationPath}");
                        }
                        else
                        {
                            // Duplicate attachment detected; skip saving
                            Console.WriteLine($"Skipped duplicate attachment: {attachment.FileName}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
