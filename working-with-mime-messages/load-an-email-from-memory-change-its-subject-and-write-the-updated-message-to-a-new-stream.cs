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
            // Sample EML content
            string emlContent = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Original Subject\r\n\r\nThis is the body.";
            byte[] emlBytes = Encoding.UTF8.GetBytes(emlContent);

            using (MemoryStream inputStream = new MemoryStream(emlBytes))
            {
                // Load the email from the memory stream
                using (MailMessage mailMessage = MailMessage.Load(inputStream))
                {
                    // Change the subject
                    mailMessage.Subject = "Updated Subject";

                    // Save the updated email to a new memory stream
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        mailMessage.Save(outputStream);
                        Console.WriteLine("Updated message saved to stream. Length: " + outputStream.Length);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
