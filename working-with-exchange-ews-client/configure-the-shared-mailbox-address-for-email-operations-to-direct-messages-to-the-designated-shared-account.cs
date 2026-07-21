using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace SharedMailboxExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // EWS service URL and user credentials
                string ewsUrl = "https://mail.example.com/EWS/Exchange.asmx";
                string userName = "user@example.com";
                string password = "password";

                // The shared mailbox to which operations should be directed
                string sharedMailboxAddress = "shared@example.com";


                // Skip external calls when placeholder credentials are used
                if (ewsUrl.Contains("example.com") || userName.Contains("example.com") || password == "password" || sharedMailboxAddress.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                NetworkCredential credentials = new NetworkCredential(userName, password);

                // Create the EWS client (implements IEWSClient) and ensure proper disposal
                using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, credentials))
                {
                    try
                    {
                        // Impersonate the shared mailbox
                        client.ImpersonateUser(ItemChoice.PrimarySmtpAddress, sharedMailboxAddress);
                        Console.WriteLine("Impersonation set to shared mailbox: " + sharedMailboxAddress);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Failed to impersonate shared mailbox: " + ex.Message);
                        return;
                    }

                    // Example: send a test email from the shared mailbox
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(userName);
                        message.To.Add(sharedMailboxAddress);
                        message.Subject = "Test email from shared mailbox";
                        message.Body = "This is a test message sent via impersonated shared mailbox.";

                        try
                        {
                            client.Send(message);
                            Console.WriteLine("Test email sent successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine("Failed to send email: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
