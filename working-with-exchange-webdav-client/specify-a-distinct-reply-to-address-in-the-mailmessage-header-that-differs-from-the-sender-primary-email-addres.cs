using System;
using Aspose.Email;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Create a new mail message and ensure it is disposed properly
                using (MailMessage message = new MailMessage())
                {
                    // Set the primary sender address
                    message.From = new MailAddress("sender@example.com", "Sender Name");

                    // Add a recipient
                    message.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));

                    // Set subject and body
                    message.Subject = "Test message with custom Reply-To";
                    message.Body = "This email demonstrates setting a distinct Reply-To address.";

                    // Specify a distinct Reply-To address via the Headers collection
                    message.Headers.Add("Reply-To", "replyto@example.com");

                    // Output the Reply-To header to verify
                    Console.WriteLine("Reply-To header set to: " + message.Headers["Reply-To"]);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return;
            }
        }
    }
}
