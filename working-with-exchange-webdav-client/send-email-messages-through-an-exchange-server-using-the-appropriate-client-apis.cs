using System.Net;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            // Top-level exception guard
            try
            {
                // Connection safety guard
                try
                {
                    // Initialize the EWS client with mailbox URI and credentials
                    using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", "username", "password"))
                    {
                        // Create a mail message
                        using (MailMessage message = new MailMessage())
                        {
                            message.From = "sender@example.com";
                            message.To = "recipient@example.com";
                            message.Subject = "Test Email";
                            message.Body = "Hello from Aspose.Email!";

                            // Send the message
                            client.Send(message);
                            Console.WriteLine("Email sent successfully.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle connection or sending errors
                    Console.Error.WriteLine("Error during Exchange operation: " + ex.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}