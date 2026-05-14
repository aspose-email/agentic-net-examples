using Aspose.Email.Clients.Exchange.Dav;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings – replace with real values.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid unwanted network calls.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and connect the Exchange client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Retrieve all contacts from the default Contacts folder.
                    MapiContact[] contacts = client.ListContacts("contacts");

                    // Prepare a list of email addresses that are not marked as Do Not Email.
                    List<string> recipientEmails = new List<string>();
                    foreach (MapiContact contact in contacts)
                    {
                        // Use reflection to safely access properties that may not exist in older library versions.
                        bool doNotEmail = false;
                        var doNotEmailProp = contact.GetType().GetProperty("DoNotEmail");
                        if (doNotEmailProp != null && doNotEmailProp.PropertyType == typeof(bool))
                        {
                            var val = doNotEmailProp.GetValue(contact);
                            if (val is bool b) doNotEmail = b;
                        }

                        string email = null;
                        var emailProp = contact.GetType().GetProperty("EmailAddress1");
                        if (emailProp != null && emailProp.PropertyType == typeof(string))
                        {
                            email = emailProp.GetValue(contact) as string;
                        }

                        if (!doNotEmail && !string.IsNullOrEmpty(email))
                        {
                            recipientEmails.Add(email);
                        }
                    }

                    if (recipientEmails.Count == 0)
                    {
                        Console.WriteLine("No eligible contacts found.");
                        return;
                    }

                    // Build the mail message.
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = "sender@domain.com";
                        message.Subject = "Important Update";
                        message.Body = "Please see the attached information.";

                        // Add filtered recipients.
                        foreach (string email in recipientEmails)
                        {
                            message.To.Add(email);
                        }

                        // Send the message.
                        client.Send(message);
                        Console.WriteLine("Message sent to {0} recipients.", recipientEmails.Count);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Exchange operations: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
