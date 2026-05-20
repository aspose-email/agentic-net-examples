using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (replace with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid external calls during CI
            if (host == "smtp.example.com" || username == "user@example.com" || password == "password")
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Prepare email addresses
            string from = "sender@example.com";
            List<string> toList = new List<string>
            {
                "valid.recipient@example.com",
                "invalid-recipient"
            };

            // Validate addresses using RFC‑5322 regex
            MailAddressCollection validRecipients = new MailAddressCollection();
            foreach (string address in toList)
            {
                if (IsValidEmail(address))
                {
                    validRecipients.Add(new MailAddress(address));
                }
                else
                {
                    Console.Error.WriteLine($"Invalid email address skipped: {address}");
                }
            }

            if (validRecipients.Count == 0)
            {
                Console.Error.WriteLine("No valid recipient addresses. Aborting send.");
                return;
            }

            // Create the mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(from);
                foreach (MailAddress recipient in validRecipients)
                {
                    message.To.Add(recipient);
                }
                message.Subject = "Test Email";
                message.Body = "This is a test email sent via Aspose.Email.";

                // Send the message using SmtpClient
                using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (SmtpFailedRecipientException ex)
                    {
                        Console.Error.WriteLine($"Failed to deliver to recipient: {ex.FailedRecipient}");
                        Console.Error.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error sending email: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Simple RFC‑5322 email validation using regular expression
    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        const string pattern =
            @"^(?("")("".+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)[0-9a-z]@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,24}))$";

        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }
}
