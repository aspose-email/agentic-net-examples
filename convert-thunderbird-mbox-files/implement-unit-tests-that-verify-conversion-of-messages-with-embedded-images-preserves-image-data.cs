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
            // Prepare test directory
            string testDir = Path.Combine(Environment.CurrentDirectory, "TestData");
            if (!Directory.Exists(testDir))
                Directory.CreateDirectory(testDir);

            // Define file paths
            string originalMsgPath = Path.Combine(testDir, "original.msg");
            string convertedEmlPath = Path.Combine(testDir, "converted.eml");

            // Ensure the original message with an embedded image exists
            if (!File.Exists(originalMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(originalMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                // Create a simple PNG header as dummy image data
                byte[] imageData = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };

                // Build the email message
                MailMessage message = new MailMessage();
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Test Message with Embedded Image";
                message.HtmlBody = "<html><body><p>Here is an image:</p><img src=\"cid:image1\"/></body></html>";

                // Add the image as an inline attachment
                var inlineAttachment = new Attachment(new MemoryStream(imageData), "image.png");
                inlineAttachment.ContentId = "image1";
                inlineAttachment.ContentDisposition.Inline = true;
                message.Attachments.Add(inlineAttachment);

                // Save the message as MSG
                message.Save(originalMsgPath);
            }

            // Load the MSG file using MapiMessage.Load
            MapiMessage mapMsg = MapiMessage.Load(originalMsgPath);

            // Convert to MailMessage
            MailConversionOptions conversionOptions = new MailConversionOptions();
            MailMessage convertedMessage = mapMsg.ToMailMessage(conversionOptions);

            // Save the converted message as EML
            convertedMessage.Save(convertedEmlPath);

            // Load the EML file back
            MailMessage loadedEml = MailMessage.Load(convertedEmlPath);

            // Find the inline attachment by ContentId
            Attachment loadedAttachment = null;
            foreach (Attachment att in loadedEml.Attachments)
            {
                if (att.ContentDisposition.Inline && att.ContentId == "image1")
                {
                    loadedAttachment = att;
                    break;
                }
            }

            if (loadedAttachment == null)
            {
                Console.Error.WriteLine("Embedded image not found after conversion.");
                return;
            }

            // Retrieve original embedded image data from the converted message
            byte[] originalData = null;
            foreach (Attachment att in convertedMessage.Attachments)
            {
                if (att.ContentDisposition.Inline && att.ContentId == "image1")
                {
                    originalData = ReadAllBytes(att.ContentStream);
                    break;
                }
            }

            if (originalData == null)
            {
                Console.Error.WriteLine("Original embedded image data could not be retrieved.");
                return;
            }

            // Retrieve loaded image data
            byte[] loadedData = ReadAllBytes(loadedAttachment.ContentStream);

            // Compare the data
            bool dataMatches = originalData.Length == loadedData.Length;
            if (dataMatches)
            {
                for (int i = 0; i < originalData.Length; i++)
                {
                    if (originalData[i] != loadedData[i])
                    {
                        dataMatches = false;
                        break;
                    }
                }
            }

            if (dataMatches)
                Console.WriteLine("Test passed: Embedded image data preserved after conversion.");
            else
                Console.Error.WriteLine("Test failed: Embedded image data differs after conversion.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static byte[] ReadAllBytes(Stream stream)
    {
        if (stream is MemoryStream ms && ms.CanSeek)
        {
            return ms.ToArray();
        }

        using (var memory = new MemoryStream())
        {
            stream.Position = 0;
            stream.CopyTo(memory);
            return memory.ToArray();
        }
    }
}
