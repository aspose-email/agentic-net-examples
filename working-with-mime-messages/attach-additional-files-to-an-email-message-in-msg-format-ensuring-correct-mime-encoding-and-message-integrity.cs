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
            // Define file paths
            string inputMsgPath = "input.msg";
            string outputMsgPath = "output.msg";
            string attachmentFilePath = "attachment.txt";

            // Ensure the attachment file exists; create a placeholder if missing
            if (!File.Exists(attachmentFilePath))
            {
                try
                {
                    File.WriteAllText(attachmentFilePath, "Placeholder attachment content.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Ensure the input MSG file exists; create a minimal placeholder if missing
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

                try
                {
                    using (MapiMessage placeholderMsg = new MapiMessage("sender@example.com", "receiver@example.com", "Sample Subject", "Sample Body"))
                    {
                        placeholderMsg.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the existing MSG file
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(inputMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            // Use using to ensure the message is disposed
            using (message)
            {
                // Read attachment bytes
                byte[] attachmentData;
                try
                {
                    attachmentData = File.ReadAllBytes(attachmentFilePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error reading attachment file: {ex.Message}");
                    return;
                }

                // Add the attachment to the message
                try
                {
                    // The Add method handles proper MIME encoding internally
                    message.Attachments.Add(Path.GetFileName(attachmentFilePath), attachmentData);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error adding attachment: {ex.Message}");
                    return;
                }

                // Save the updated message
                try
                {
                    // Ensure the output directory exists
                    string outputDirectory = Path.GetDirectoryName(outputMsgPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    message.Save(outputMsgPath);
                    Console.WriteLine($"Message saved with attachment to: {outputMsgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
