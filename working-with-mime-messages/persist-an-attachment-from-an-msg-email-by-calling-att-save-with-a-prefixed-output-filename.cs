using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths and prefix
            string inputMsgPath = "sample.msg";
            string outputDirectory = "output";
            string fileNamePrefix = "saved_";

            // Ensure the output directory exists
            Directory.CreateDirectory(outputDirectory);

            // Guard input file existence
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                // Create a minimal placeholder MSG file
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("Placeholder Subject", "Placeholder Body", "sender@example.com", "receiver@example.com"))
                    {
                        placeholder.Save(inputMsgPath);
                        Console.WriteLine($"Placeholder MSG created at '{inputMsgPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and extract attachments
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputMsgPath))
                {
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        // Determine output file name with prefix
                        string originalFileName = attachment.FileName ?? "attachment";
                        string outputFilePath = Path.Combine(outputDirectory, fileNamePrefix + originalFileName);

                        // Save the attachment
                        try
                        {
                            attachment.Save(outputFilePath);
                            Console.WriteLine($"Attachment saved to '{outputFilePath}'.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{originalFileName}': {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
