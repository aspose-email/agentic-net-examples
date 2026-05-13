using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Raw MIME string to be parsed
            string rawMime = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Test\r\n\r\nThis is the body.";

            // Convert the string to a byte array
            byte[] mimeBytes = Encoding.UTF8.GetBytes(rawMime);

            // Load the MailMessage from the MIME stream
            using (MemoryStream mimeStream = new MemoryStream(mimeBytes))
            {
                using (MailMessage message = MailMessage.Load(mimeStream))
                {
                    // Example manipulation: display subject and body
                    Console.WriteLine("Subject: " + message.Subject);
                    Console.WriteLine("Body: " + message.Body);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
