using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "input.msg";
            string outputDirectory = "output";

            // Ensure input MSG exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                using (MailMessage placeholder = new MailMessage())
                {
                    placeholder.From = "placeholder@example.com";
                    placeholder.To = "recipient@example.com";
                    placeholder.Subject = "Placeholder Message";
                    placeholder.Body = "This is a placeholder MSG file.";
                    placeholder.Save(msgPath, SaveOptions.DefaultMsgUnicode);
                }
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                foreach (LinkedResource resource in message.LinkedResources)
                {
                    if (resource.ContentStream != null)
                    {
                        string fileName = string.IsNullOrEmpty(resource.ContentId) ? "resource.bin" : resource.ContentId + ".bin";
                        string outputPath = Path.Combine(outputDirectory, fileName);

                        // Save the content stream to a file
                        using (FileStream fileStream = File.Create(outputPath))
                        {
                            resource.ContentStream.CopyTo(fileStream);
                        }

                        Console.WriteLine($"Linked resource saved to: {outputPath}");
                        // Process only the first linked resource
                        break;
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
