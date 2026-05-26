using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP settings
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
                return;
            }

            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    // Create a mail message with duplicate recipients
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress("sender@example.com");

                        // Add duplicates
                        message.To.Add(new MailAddress("recipient1@example.com"));
                        message.To.Add(new MailAddress("recipient2@example.com"));
                        message.To.Add(new MailAddress("recipient1@example.com")); // duplicate

                        message.CC.Add(new MailAddress("cc1@example.com"));
                        message.CC.Add(new MailAddress("cc1@example.com")); // duplicate

                        message.Bcc.Add(new MailAddress("bcc1@example.com"));
                        message.Bcc.Add(new MailAddress("recipient2@example.com")); // duplicate across lists

                        message.Subject = "Test Email";
                        message.Body = "This email demonstrates automatic duplicate recipient removal.";

                        // Remove duplicate recipients across To, CC, BCC
                        RemoveDuplicateRecipients(message);

                        // Send the message
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during SMTP operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Removes duplicate addresses from To, CC, and BCC collections while preserving the first occurrence.
    private static void RemoveDuplicateRecipients(MailMessage message)
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        RemoveDuplicatesFromCollection(message.To, seen);
        RemoveDuplicatesFromCollection(message.CC, seen);
        RemoveDuplicatesFromCollection(message.Bcc, seen);
    }

    private static void RemoveDuplicatesFromCollection(MailAddressCollection collection, HashSet<string> seen)
    {
        var unique = new MailAddressCollection();
        foreach (MailAddress address in collection)
        {
            if (seen.Add(address.Address))
            {
                unique.Add(address);
            }
        }
        collection.Clear();
        foreach (MailAddress address in unique)
        {
            collection.Add(address);
        }
    }
}
