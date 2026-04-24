using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "nested_alternative.eml";
            string msgPath = "nested_alternative.msg";

            // Ensure the input EML file exists; create a minimal placeholder if missing.
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
                    string placeholder = @"From: sender@example.com
To: recipient@example.com
Subject: Test Nested Multipart/Alternative
MIME-Version: 1.0
Content-Type: multipart/mixed; boundary=""mixed-boundary""

--mixed-boundary
Content-Type: multipart/alternative; boundary=""alt-boundary""

--alt-boundary
Content-Type: text/plain; charset=""utf-8""

Plain text version.

--alt-boundary
Content-Type: text/html; charset=""utf-8""

<html><body><p>HTML version.</p></body></html>

--alt-boundary--
--mixed-boundary--";
                    File.WriteAllText(emlPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the EML message and save it as MSG.
            try
            {
                using (MailMessage emlMessage = MailMessage.Load(emlPath))
                {
                    emlMessage.Save(msgPath, SaveOptions.DefaultMsg);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading or saving message: {ex.Message}");
                return;
            }

            // Load the MSG as a MapiMessage and convert back to MailMessage.
            try
            {
                using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
                {
                    MailConversionOptions conversionOptions = new MailConversionOptions();
                    using (MailMessage convertedMessage = mapiMessage.ToMailMessage(conversionOptions))
                    {
                        Console.WriteLine("Converted message body:");
                        Console.WriteLine(convertedMessage.Body);
                        Console.WriteLine($"IsBodyHtml: {convertedMessage.IsBodyHtml}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error converting MSG to MailMessage: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
