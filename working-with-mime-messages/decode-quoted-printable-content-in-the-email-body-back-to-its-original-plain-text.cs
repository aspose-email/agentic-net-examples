using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
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
                                         "Subject: Test quoted‑printable\r\n" +
                                         "Content-Transfer-Encoding: quoted-printable\r\n" +
                                         "Content-Type: text/plain; charset=UTF-8\r\n\r\n" +
                                         "Hello=2C=20World=21\r\n";
                    File.WriteAllText(emlPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the email message. Aspose.Email automatically decodes quoted‑printable content.
            using (MailMessage message = MailMessage.Load(emlPath))
            {
                string decodedBody = message.Body;
                Console.WriteLine("Decoded body:");
                Console.WriteLine(decodedBody);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
