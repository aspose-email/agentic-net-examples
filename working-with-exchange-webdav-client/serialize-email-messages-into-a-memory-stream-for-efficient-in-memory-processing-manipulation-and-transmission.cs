using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

namespace EmailSerializationSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Create a simple email message
                MailMessage message = new MailMessage();
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Test Message";
                message.Body = "This is a test email.";

                // Serialize the message into a memory stream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Use default EML save options for in‑memory saving
                    SaveOptions emlOptions = SaveOptions.DefaultEml;
                    message.Save(memoryStream, emlOptions);

                    // Reset the stream position for any subsequent read operations
                    memoryStream.Position = 0;

                    // Example: output the size of the serialized message
                    Console.WriteLine($"Serialized message size: {memoryStream.Length} bytes");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
