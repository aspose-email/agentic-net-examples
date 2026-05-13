using System;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Create a new email message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Test Email";
            message.Body = "This is a test email.";

            // Generate a unique Message-Id using a GUID
            string guid = Guid.NewGuid().ToString();
            // Set the MessageId property (include angle brackets and a domain)
            message.MessageId = $"<{guid}@example.com>";

            // Output the generated Message-Id
            Console.WriteLine("Message-Id set to: " + message.MessageId);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return;
        }
    }
}
