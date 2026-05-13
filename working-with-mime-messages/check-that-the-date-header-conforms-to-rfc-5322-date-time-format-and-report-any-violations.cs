using System;
using System.Globalization;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "sample.eml";

            // Ensure the input file exists; create a minimal placeholder if missing
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
                    using (MailMessage placeholderMsg = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholderMsg.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                string placeholderContent = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Test\r\nDate: Tue, 9 Jan 2001 23:40:00 -0800\r\n\r\nBody.";
                try
                {
                    File.WriteAllText(emlPath, placeholderContent);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                    return;
                }
            }

            MailMessage message;
            try
            {
                message = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load email: {ex.Message}");
                return;
            }

            using (message)
            {
                string dateHeader = message.Headers[HeaderType.Date];
                if (string.IsNullOrEmpty(dateHeader))
                {
                    Console.WriteLine("Date header is missing.");
                    return;
                }

                string[] formats = new[]
                {
                    "ddd, d MMM yyyy HH:mm:ss zzz",
                    "ddd, dd MMM yyyy HH:mm:ss zzz",
                    "ddd, d MMM yy HH:mm:ss zzz",
                    "ddd, dd MMM yy HH:mm:ss zzz"
                };

                bool isValid = DateTimeOffset.TryParseExact(dateHeader, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
                if (isValid)
                {
                    Console.WriteLine("Date header conforms to RFC 5322 format.");
                }
                else
                {
                    Console.WriteLine($"Date header violates RFC 5322 format: \"{dateHeader}\"");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
