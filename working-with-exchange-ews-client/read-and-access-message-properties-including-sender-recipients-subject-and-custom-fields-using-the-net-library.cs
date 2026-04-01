using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define the path to the email file
            string emailFilePath = "sample.eml";

            // Ensure the file exists; if not, create a minimal placeholder
            if (!File.Exists(emailFilePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emailFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    string placeholderContent = "Subject: Placeholder Email\r\nFrom: sender@example.com\r\nTo: recipient@example.com\r\n\r\nThis is a placeholder email body.";
                    File.WriteAllText(emailFilePath, placeholderContent);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine("Failed to create placeholder email file: " + ioEx.Message);
                    return;
                }
            }

            // Load the email message
            using (MailMessage message = MailMessage.Load(emailFilePath))
            {
                // Access standard properties
                string subject = message.Subject;
                string from = message.From != null ? message.From.ToString() : string.Empty;
                string sender = message.Sender != null ? message.Sender.ToString() : string.Empty;

                // Recipients (To, Cc, Bcc)
                string toRecipients = string.Empty;
                if (message.To != null)
                {
                    foreach (MailAddress address in message.To)
                    {
                        toRecipients += address.ToString() + "; ";
                    }
                }

                string ccRecipients = string.Empty;
                if (message.CC != null)
                {
                    foreach (MailAddress address in message.CC)
                    {
                        ccRecipients += address.ToString() + "; ";
                    }
                }

                string bccRecipients = string.Empty;
                if (message.Bcc != null)
                {
                    foreach (MailAddress address in message.Bcc)
                    {
                        bccRecipients += address.ToString() + "; ";
                    }
                }

                // Access custom header fields
                string customHeaderValue = string.Empty;
                if (message.Headers != null && message.Headers.Contains("X-Custom-Header"))
                {
                    customHeaderValue = message.Headers["X-Custom-Header"];
                }

                // Output the retrieved information
                Console.WriteLine("Subject: " + subject);
                Console.WriteLine("From: " + from);
                Console.WriteLine("Sender: " + sender);
                Console.WriteLine("To: " + toRecipients);
                Console.WriteLine("CC: " + ccRecipients);
                Console.WriteLine("BCC: " + bccRecipients);
                Console.WriteLine("Custom Header (X-Custom-Header): " + customHeaderValue);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
