using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Create a simple MailMessage in memory
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("sender@example.com");
            mailMessage.To.Add(new MailAddress("recipient@example.com"));
            mailMessage.Subject = "Sample Subject";
            mailMessage.Body = "This is a sample email body.";

            // Convert MailMessage to MapiMessage
            MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

            // Create a PST in a memory stream (Unicode format)
            using (MemoryStream pstStream = new MemoryStream())
            {
                using (PersonalStorage pst = PersonalStorage.Create(pstStream, FileFormatVersion.Unicode))
                {
                    // Add the message to the root folder of the PST
                    string entryId = pst.RootFolder.AddMessage(mapiMessage);

                    // Save the added message directly to another stream
                    using (MemoryStream messageStream = new MemoryStream())
                    {
                        pst.SaveMessageToStream(entryId, messageStream);

                        // For demonstration, output the size of the saved message
                        Console.WriteLine($"Message saved to stream. Size: {messageStream.Length} bytes");
                    }
                }

                // Optionally, the PST stream now contains the PST data
                Console.WriteLine($"PST created in memory. Size: {pstStream.Length} bytes");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
