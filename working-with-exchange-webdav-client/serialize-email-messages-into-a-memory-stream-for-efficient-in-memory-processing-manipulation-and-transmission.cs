using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test Email";
                message.Body = "This is a test email.";

                // Serialize the message into a memory stream
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Save the message in EML format to the stream
                    message.Save(memoryStream, SaveOptions.DefaultEml);

                    // Reset the stream position for further processing if needed
                    memoryStream.Position = 0;

                    // Example: obtain the serialized bytes
                    byte[] emailBytes = memoryStream.ToArray();
                    Console.WriteLine($"Serialized email size: {emailBytes.Length} bytes");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
