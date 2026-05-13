using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string messagePath = "sample.eml";

            // Ensure the input file exists; create a minimal placeholder if missing.
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(messagePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    string placeholder = "From: sender@example.com\r\n" +
                                         "To: recipient@example.com\r\n" +
                                         "Subject: Sample Message\r\n" +
                                         "Content-Transfer-Encoding: quoted-printable\r\n" +
                                         "\r\n" +
                                         "Hello=20World=21\r\n";
                    File.WriteAllText(messagePath, placeholder);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ioEx.Message}");
                    return;
                }
            }

            // Load the email message and decode quoted‑printable body to plain text.
            using (MailMessage mailMessage = MailMessage.Load(messagePath))
            {
                string decodedBody = mailMessage.Body;
                Console.WriteLine("Decoded Body:");
                Console.WriteLine(decodedBody);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
