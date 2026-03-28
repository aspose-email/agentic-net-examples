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
            string inputPath = "sample.eml";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            string outputPath = "extracted_image.msg";

            try
            {
                using (MailMessage originalMessage = MailMessage.Load(inputPath))
                {
                    if (originalMessage.LinkedResources.Count == 0)
                    {
                        Console.Error.WriteLine("No linked resources found in the email.");
                        return;
                    }

                    LinkedResource linked = originalMessage.LinkedResources[0];

                    using (MemoryStream imageStream = new MemoryStream())
                    {
                        linked.ContentStream.CopyTo(imageStream);
                        imageStream.Position = 0;

                        using (MailMessage extractedMessage = new MailMessage())
                        {
                            extractedMessage.From = "extracted@example.com";
                            extractedMessage.To = "extracted@example.com";
                            extractedMessage.Subject = "Extracted Image";

                            Attachment attachment = new Attachment(imageStream, linked.ContentType);
                            extractedMessage.Attachments.Add(attachment);

                            extractedMessage.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing email: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
