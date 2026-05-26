using Aspose.Email.Clients;
using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        // Placeholder SMTP credentials – replace with real values before running.
        string smtpHost = "YOUR_SMTP_HOST";
        int smtpPort = 587; // Common SMTP port; adjust if needed.
        string smtpUser = "YOUR_SMTP_USERNAME";
        string smtpPass = "YOUR_SMTP_PASSWORD";

        // Guard against placeholder values to avoid accidental network calls.
        if (smtpHost.StartsWith("YOUR_") || smtpUser.StartsWith("YOUR_") || smtpPass.StartsWith("YOUR_"))
        {
            Console.Error.WriteLine("Please provide valid SMTP credentials.");
            return;
        }

        // Create the SMTP client.
        using var client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass);
        client.SecurityOptions = SecurityOptions.Auto;

        // Build the email message.
        var message = new MailMessage
        {
            From = "sender@example.com",
            Subject = "Custom MIME Type Example"
        };
        message.To.Add("recipient@example.com");

        // Optional plain text body.
        message.Body = "Please see the attached custom JSON payload.";

        // Create a custom MIME part with the desired content type.
        string jsonPayload = "{\"key\":\"value\"}";
        var payloadBytes = Encoding.UTF8.GetBytes(jsonPayload);
        using var payloadStream = new MemoryStream(payloadBytes);

        var customContentType = new ContentType("application/vnd.custom+json")
        {
            Name = "payload.json"
        };

        var customAttachment = new Attachment(payloadStream, customContentType);
        message.Attachments.Add(customAttachment);

        try
        {
            client.Send(message);
            Console.WriteLine("Message sent successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to send message: {ex.Message}");
        }
    }
}
