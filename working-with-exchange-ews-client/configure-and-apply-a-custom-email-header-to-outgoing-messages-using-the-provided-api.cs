using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define EWS service URL and credentials
            string ewsUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("user@example.com", "password");

            // Guard: skip external calls when placeholder credentials are used
            bool placeholders = ewsUrl.Contains("outlook.office365.com") &&
                                credentials.UserName == "user@example.com" &&
                                credentials.Password == "password";

            if (placeholders)
            {
                Console.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Initialize the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, credentials, proxy: null))
            {
                // Create a simple email message
                MailMessage message = new MailMessage();
                message.From = new MailAddress("user@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Test Email with Custom Header";
                message.Body = "This email was sent using Aspose.Email with a custom EWS header.";

                // Add a custom header to the email message
                message.Headers.Add("X-Custom-Header", "MyValue");

                // Send the message via EWS
                client.Send(message);
                Console.WriteLine("Email sent successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
