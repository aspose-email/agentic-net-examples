using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        string inputMsgPath = "input.msg";
        string outputFolder = "ExtractedAttachments";

        // Verify input MSG file exists
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

            Console.Error.WriteLine($"Input file '{inputMsgPath}' not found.");
            return;
        }

        // Ensure output directory exists
        try
        {
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create output folder: {ex.Message}");
            return;
        }

        try
        {
            // Load MSG preserving TNEF attachments
            MapiMessage mapMsg = MapiMessage.Load(inputMsgPath);

            // Convert to MailMessage for attachment handling
            MailConversionOptions conversionOptions = new MailConversionOptions();
            using (MailMessage mailMsg = mapMsg.ToMailMessage(conversionOptions))
            {
                // Extract each attachment
                foreach (Attachment attachment in mailMsg.Attachments)
                {
                    // Ensure Name is set (required for validation)
                    if (string.IsNullOrEmpty(attachment.Name))
                    {
                        attachment.Name = attachment.Name ?? "attachment.bin";
                    }

                    string outputPath = Path.Combine(outputFolder, attachment.Name);
                    attachment.Save(outputPath);
                    Console.WriteLine($"Saved attachment: {outputPath}");
                }

                // Example edit: append text to the first attachment if it is a text file
                if (mailMsg.Attachments.Count > 0)
                {
                    Attachment first = mailMsg.Attachments[0];
                    if (first.ContentType.MediaType.StartsWith("text", StringComparison.OrdinalIgnoreCase))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            first.Save(ms);
                            ms.Position = 0;
                            using (StreamReader reader = new StreamReader(ms))
                            {
                                string content = reader.ReadToEnd();
                                content += "\nEdited by Aspose.Email.";
                                byte[] editedBytes = System.Text.Encoding.UTF8.GetBytes(content);
                                using (MemoryStream editedStream = new MemoryStream(editedBytes))
                                {
                                    Attachment edited = new Attachment(editedStream, "edited.txt");
                                    edited.Name = "edited.txt";
                                    mailMsg.Attachments.Remove(first);
                                    mailMsg.Attachments.Add(edited);
                                }
                            }
                        }
                    }
                }

                // Save modified message back to MSG format
                string modifiedMsgPath = Path.Combine(outputFolder, "modified.msg");
                mailMsg.Save(modifiedMsgPath, SaveOptions.DefaultMsgUnicode);
                Console.WriteLine($"Modified message saved: {modifiedMsgPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing MSG: {ex.Message}");
        }
    }
}
