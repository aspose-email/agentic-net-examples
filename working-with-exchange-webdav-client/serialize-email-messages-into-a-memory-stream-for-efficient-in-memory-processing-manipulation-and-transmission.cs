using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a simple email message
            MailMessage message = new MailMessage();
            try
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Sample Email";
                message.Body = "This is a test email serialized into a memory stream.";

                // Serialize the message into a memory stream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Save the message in EML format to the stream
                    message.Save(memoryStream, SaveOptions.DefaultEml);

                    // Reset stream position to the beginning for further processing
                    memoryStream.Position = 0;

                    // Example: read the serialized bytes (could be sent over network, etc.)
                    byte[] emailBytes = memoryStream.ToArray();
                    Console.WriteLine($"Serialized email size: {emailBytes.Length} bytes");
                }
            }
            finally
            {
                // Ensure the MailMessage is disposed
                message.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
