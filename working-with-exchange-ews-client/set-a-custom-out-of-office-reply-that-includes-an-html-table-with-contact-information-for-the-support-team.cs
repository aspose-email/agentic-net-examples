using Aspose.Email.Clients.Exchange;
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
            // Connection parameters (replace with actual values)
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve messages from the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages();
                if (messages == null || messages.Count == 0)
                {
                    Console.Error.WriteLine("No messages found in the mailbox.");
                    return;
                }

                // Take the first message as the reference for the OOF reply
                ExchangeMessageInfo originalInfo = messages[0];
                MailMessage originalMessage = client.FetchMessage(originalInfo.UniqueUri);

                // Compose the out‑of‑office reply with an HTML table
                MailMessage replyMessage = new MailMessage();
                replyMessage.From = new MailAddress(username);
                replyMessage.To.Add(originalMessage.From);
                replyMessage.Subject = "Out of Office: " + originalMessage.Subject;
                replyMessage.HtmlBody = @"
<html>
<body>
<p>Thank you for your email. Our support team will get back to you shortly.</p>
<table border='1' cellpadding='5' cellspacing='0'>
    <tr><th>Support Team</th><th>Email</th></tr>
    <tr><td>Technical Support</td><td>techsupport@example.com</td></tr>
    <tr><td>Customer Service</td><td>service@example.com</td></tr>
</table>
</body>
</html>";

                // Send the reply using the EWS client
                client.Reply(replyMessage, originalInfo);
                Console.WriteLine("Out of office reply sent successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
