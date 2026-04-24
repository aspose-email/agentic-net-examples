using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Tools;

class Program
{
    static void Main()
    {
        try
        {
            // Sample EML content
            string emlContent = "From: sender@example.com\r\nTo: receiver@example.com\r\nSubject: Test Email\r\n\r\nThis is a test email body.";
            byte[] emlBytes = Encoding.UTF8.GetBytes(emlContent);

            using (MemoryStream emlStream = new MemoryStream(emlBytes))
            {
                MailMessage mailMessage = MailMessage.Load(emlStream);
                using (mailMessage)
                {
                    HtmlSaveOptions htmlOptions = new HtmlSaveOptions();

                    using (MemoryStream htmlStream = new MemoryStream())
                    {
                        mailMessage.Save(htmlStream, htmlOptions);

                        // The HTML result is now in htmlStream
                        string htmlResult = Encoding.UTF8.GetString(htmlStream.ToArray());
                        Console.WriteLine("Converted HTML:");
                        Console.WriteLine(htmlResult);
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
