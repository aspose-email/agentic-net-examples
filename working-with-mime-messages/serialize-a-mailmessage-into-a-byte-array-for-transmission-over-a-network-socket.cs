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
            // Create a simple mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test Email";
                message.Body = "This is a test email.";

                // Serialize the message to a byte array in MIME (EML) format
                byte[] mimeBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Use EmlSaveOptions as required by Aspose.Email validation
                    message.Save(memoryStream, SaveOptions.DefaultEml);
                    mimeBytes = memoryStream.ToArray();
                }

                // Example output: size of the serialized data
                Console.WriteLine($"Serialized message size: {mimeBytes.Length} bytes");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
