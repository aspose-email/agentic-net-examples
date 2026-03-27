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
            // Create a simple MailMessage
            using (Aspose.Email.MailMessage mailMessage = new Aspose.Email.MailMessage(
                "sender@example.com",
                "recipient@example.com",
                "Sample Subject",
                "This is the body of the email."))
            {
                // Convert MailMessage to MapiMessage
                using (Aspose.Email.Mapi.MapiMessage mapiMessage = Aspose.Email.Mapi.MapiMessage.FromMailMessage(mailMessage))
                {
                    // Serialize MapiMessage to a memory stream
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        mapiMessage.Save(memoryStream);
                        memoryStream.Position = 0;

                        Console.WriteLine($"Serialized message size: {memoryStream.Length} bytes");

                        // Optionally, load the message back from the stream
                        using (Aspose.Email.MailMessage loadedMessage = Aspose.Email.MailMessage.Load(memoryStream))
                        {
                            Console.WriteLine($"Loaded Subject: {loadedMessage.Subject}");
                        }
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