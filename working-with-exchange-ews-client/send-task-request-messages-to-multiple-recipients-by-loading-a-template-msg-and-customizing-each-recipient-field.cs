using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details
            string hostUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Initialize the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(hostUri, username, password))
            {
                // Path to the MSG template
                string templatePath = "Template.msg";

                if (!File.Exists(templatePath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(templatePath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"Template file not found: {templatePath}");
                    return;
                }

                // Recipients to send the task request to
                List<string> recipients = new List<string>
                {
                    "alice@example.com",
                    "bob@example.com",
                    "carol@example.com"
                };

                foreach (string recipient in recipients)
                {
                    try
                    {
                        // Load the template message
                        MailMessage message = MailMessage.Load(templatePath);

                        // Set the current recipient
                        message.To.Clear();
                        message.To.Add(recipient);

                        // Optional: customize subject per recipient
                        message.Subject = $"Task Request for {recipient}";

                        // Send the message
                        client.Send(message);
                        Console.WriteLine($"Message sent to {recipient}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send to {recipient}: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
