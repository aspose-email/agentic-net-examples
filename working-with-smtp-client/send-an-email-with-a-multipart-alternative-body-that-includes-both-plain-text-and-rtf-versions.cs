using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare the plain‑text and RTF bodies
            string plainText = "This is the plain‑text version of the email.";
            string rtfText = @"{\rtf1\ansi\deff0{\fonttbl{\f0\fswiss Helvetica;}}
\viewkind4\uc1\pard\fs20 This is the \b RTF \b0 version of the email.\par}";

            // Create the mail message with a default plain‑text body
            using (MailMessage message = new MailMessage("from@example.com", "to@example.com", "Multipart/Alternative Example", plainText))
            {
                // Add an alternate view for the RTF version
                AlternateView rtfView = AlternateView.CreateAlternateViewFromString(rtfText, new ContentType("application/rtf"));
                message.AlternateViews.Add(rtfView);

                // Placeholder SMTP configuration
                string smtpHost = "smtp.example.com";
                int smtpPort = 587;
                string smtpUser = "username";
                string smtpPass = "password";

                // Guard against placeholder credentials/hosts
                if (smtpHost.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder SMTP configuration detected; skipping actual send.");
                    return;
                }

                // Send the message using SmtpClient
                try
                {
                    using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                    {
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
