using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // EWS connection parameters
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Path to the existing email file
            string emlPath = "message.eml";

            // Disclaimer to prepend
            string disclaimer = "This email is confidential.\n\n";

            // Verify the email file exists
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

                Console.Error.WriteLine($"File not found: {emlPath}");
                return;
            }

            // Load the existing email message
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load email: {ex.Message}");
                return;
            }

            // Update the body to include the disclaimer
            mailMessage.Body = disclaimer + mailMessage.Body;

            // Create and use the EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    try
                    {
                        client.Send(mailMessage);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
