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
            // Raw RFC822 email string
            string rawEmail = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Sample Email\r\nDate: Fri, 1 Jan 2021 12:34:56 +0000\r\n\r\nThis is the body of the email.";

            // Convert the string to a memory stream
            byte[] emailBytes = Encoding.UTF8.GetBytes(rawEmail);
            using (MemoryStream emailStream = new MemoryStream(emailBytes))
            {
                // Load the MailMessage from the stream
                using (MailMessage message = MailMessage.Load(emailStream))
                {
                    // Display some properties to verify successful parsing
                    Console.WriteLine("From: " + message.From);
                    Console.WriteLine("To: " + message.To);
                    Console.WriteLine("Subject: " + message.Subject);
                    Console.WriteLine("Date: " + message.Date);
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
