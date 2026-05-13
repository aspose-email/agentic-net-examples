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

            // Ensure the file exists; create a minimal placeholder if it does not.
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
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the MIME message.
            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                // Validate required headers.
                bool hasFrom = mailMessage.From != null && !string.IsNullOrEmpty(mailMessage.From.Address);
                bool hasTo = mailMessage.To != null && mailMessage.To.Count > 0;
                bool hasSubject = !string.IsNullOrEmpty(mailMessage.Subject);

                if (hasFrom && hasTo && hasSubject)
                {
                    Console.WriteLine("All required headers (From, To, Subject) are present.");
                }
                else
                {
                    if (!hasFrom)
                        Console.WriteLine("Missing required header: From");
                    if (!hasTo)
                        Console.WriteLine("Missing required header: To");
                    if (!hasSubject)
                        Console.WriteLine("Missing required header: Subject");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
