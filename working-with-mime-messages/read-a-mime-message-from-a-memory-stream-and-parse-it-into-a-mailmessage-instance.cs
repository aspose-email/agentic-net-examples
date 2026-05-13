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
            // Sample MIME content representing an email message
            string mimeContent = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Test Email\r\n\r\nThis is the body of the email.";
            byte[] mimeBytes = Encoding.UTF8.GetBytes(mimeContent);

            using (MemoryStream memoryStream = new MemoryStream(mimeBytes))
            {
                using (MailMessage mailMessage = MailMessage.Load(memoryStream))
                {
                    Console.WriteLine("Subject: " + mailMessage.Subject);
                    Console.WriteLine("From: " + mailMessage.From);
                    Console.WriteLine("To: " + string.Join(", ", mailMessage.To));
                    Console.WriteLine("Body: " + mailMessage.Body);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
