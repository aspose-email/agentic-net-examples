using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // EWS service URL and admin credentials
            string ewsUrl = "https://mail.example.com/EWS/Exchange.asmx";
            string adminUser = "admin@example.com";
            string adminPassword = "adminPassword";

            // User to impersonate
            string impersonatedUser = "user@example.com";

            // Create the EWS client
            using (IEWSClient ewsClient = EWSClient.GetEWSClient(ewsUrl, adminUser, adminPassword))
            {
                // Impersonate the alternate user
                ewsClient.ImpersonateUser(ItemChoice.PrimarySmtpAddress, impersonatedUser);

                // Compose a simple email
                MailMessage message = new MailMessage();
                message.From = new MailAddress(impersonatedUser);
                message.To.Add(new MailAddress(impersonatedUser));
                message.Subject = "Impersonation Test";
                message.Body = "This email was sent while impersonating another user.";

                // Send the email
                ewsClient.Send(message);

                // Reset impersonation (optional, ensures original context is restored)
                ewsClient.ResetImpersonation();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
