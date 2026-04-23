using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // Define input MBOX file and output directory.
            string inputMboxPath = "input.mbox";
            string outputDirectory = "ExtractedBodies";

            // Verify that the input file exists.
            if (!File.Exists(inputMboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputMboxPath}");
                return;
            }

            // Ensure the output directory exists.
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Create a reader for the MBOX storage.
            try
            {
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(inputMboxPath, new MboxLoadOptions()))
                {
                    int messageIndex = 0;
                    foreach (MailMessage message in mboxReader.EnumerateMessages())
                    {
                        // Filter messages by the desired priority (e.g., High).
                        if (message.Priority == MailPriority.High)
                        {
                            // Build a safe file name using the message index.
                            string safeFileName = $"Message_{messageIndex}_Body.txt";
                            string outputPath = Path.Combine(outputDirectory, safeFileName);

                            // Write the message body to a text file.
                            try
                            {
                                using (StreamWriter writer = new StreamWriter(outputPath, false))
                                {
                                    writer.Write(message.Body);
                                }
                                Console.WriteLine($"Saved body of message #{messageIndex} to '{outputPath}'.");
                            }
                            catch (Exception writeEx)
                            {
                                Console.Error.WriteLine($"Failed to write file '{outputPath}': {writeEx.Message}");
                            }
                        }
                        messageIndex++;
                    }
                }
            }
            catch (Exception readEx)
            {
                Console.Error.WriteLine($"Error reading MBOX file: {readEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
