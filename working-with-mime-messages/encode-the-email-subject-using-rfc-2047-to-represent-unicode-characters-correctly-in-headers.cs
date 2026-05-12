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
            // Prepare output directory and file path
            string outputDirectory = "Output";
            string outputPath = Path.Combine(outputDirectory, "EncodedSubject.eml");

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a mail message with a Unicode subject
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                // Subject contains non‑ASCII characters
                message.Subject = "Привет мир – Hello World";
                // Set UTF‑8 encoding so Aspose.Email encodes the subject using RFC 2047 when saving
                message.SubjectEncoding = Encoding.UTF8;

                // Save the message (ASP.NET will apply RFC 2047 encoding to the subject)
                message.Save(outputPath);
                Console.WriteLine("Message saved to: " + outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
