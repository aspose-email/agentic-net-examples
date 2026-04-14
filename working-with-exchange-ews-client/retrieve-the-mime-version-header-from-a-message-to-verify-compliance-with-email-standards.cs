using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";

            // Ensure the EML file exists; create a minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                using (FileStream createStream = File.Create(emlPath))
                {
                    string placeholder = "MIME-Version: 1.0\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(placeholder);
                    createStream.Write(bytes, 0, bytes.Length);
                }
            }

            // Load the message and retrieve the MIME-Version header
            using (FileStream readStream = File.OpenRead(emlPath))
            {
                using (MailMessage message = MailMessage.Load(readStream))
                {
                    string mimeVersion = message.Headers[HeaderType.MIMEVersion];
                    Console.WriteLine("MIME-Version header value: " + mimeVersion);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
