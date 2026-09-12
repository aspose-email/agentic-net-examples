using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    // Author: Aspose.Email example – loads a MSG file, displays its content, saves attachments, and creates a copy.
    static void Main(string[] args)
    {
        try
        {
            // Determine input MSG file path (first argument or default).
            string inputPath = args.Length > 0 ? args[0] : "input.msg";

            // Guard: ensure the MSG file exists.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the MSG file.
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Display basic properties.
                Console.WriteLine("Subject: " + msg.Subject);
                Console.WriteLine("From: " + msg.SenderName);
                Console.WriteLine("Body: " + msg.Body);

                // Prepare attachment output folder.
                string attachmentFolder = Path.Combine(Path.GetDirectoryName(inputPath) ?? "", "Attachments");
                if (!Directory.Exists(attachmentFolder))
                {
                    Directory.CreateDirectory(attachmentFolder);
                }

                // Save each attachment to the folder.
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    string attachmentPath = Path.Combine(attachmentFolder, attachment.FileName);
                    try
                    {
                        attachment.Save(attachmentPath);
                        Console.WriteLine($"Saved attachment: {attachmentPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
                    }
                }

                // Example business logic: prepend a tag to the subject.
                string newSubject = "[Processed] " + msg.Subject;
                msg.Subject = newSubject;

                // Save the modified message as a new file.
                string outputPath = Path.Combine(Path.GetDirectoryName(inputPath) ?? "", "processed.msg");
                try
                {
                    msg.Save(outputPath);
                    Console.WriteLine($"Modified message saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save modified message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
