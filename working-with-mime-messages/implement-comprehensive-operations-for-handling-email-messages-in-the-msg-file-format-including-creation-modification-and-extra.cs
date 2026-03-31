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
            // Define paths
            string msgPath = "sample.msg";
            string attachmentPath = "attachment.txt";
            string outputDirectory = "output";
            string modifiedMsgPath = "modified.msg";

            // Ensure output directory exists
            Directory.CreateDirectory(outputDirectory);

            // Guard and create placeholder attachment if missing
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Placeholder attachment content.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Guard and create placeholder MSG if missing
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

                try
                {
                    using (MapiMessage placeholderMsg = new MapiMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Placeholder Subject",
                        "This is a placeholder message."))
                    {
                        placeholderMsg.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load existing MSG
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (message)
            {
                // Extract existing attachments
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    try
                    {
                        string savedPath = Path.Combine(outputDirectory, attachment.FileName);
                        attachment.Save(savedPath);
                        Console.WriteLine($"Extracted attachment: {savedPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
                    }
                }

                // Add a new attachment to the message
                try
                {
                    byte[] attachmentData = File.ReadAllBytes(attachmentPath);
                    message.Attachments.Add(Path.GetFileName(attachmentPath), attachmentData);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add attachment: {ex.Message}");
                }

                // Modify some properties
                message.Subject = "Modified Subject";
                message.Body = "The message body has been updated.";

                // Save the modified MSG
                try
                {
                    message.Save(modifiedMsgPath);
                    Console.WriteLine($"Modified MSG saved to: {modifiedMsgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save modified MSG: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
