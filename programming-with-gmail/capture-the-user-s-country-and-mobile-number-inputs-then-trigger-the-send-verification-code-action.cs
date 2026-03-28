using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            Console.Write("Enter your country: ");
            string country = Console.ReadLine();

            Console.Write("Enter your mobile number: ");
            string mobileNumber = Console.ReadLine();

            // Generate a simple verification code
            Random rnd = new Random();
            int verificationCode = rnd.Next(100000, 999999);

            // Initialize Gmail client with dummy credentials
            using (IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com"))
            {
                // Create the email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = "no-reply@example.com";
                    message.To = "recipient@example.com";
                    message.Subject = "Your Verification Code";
                    message.Body = $"Country: {country}\nMobile: {mobileNumber}\nVerification Code: {verificationCode}";

                    // Send the message
                    gmailClient.SendMessage(message);
                }
            }

            Console.WriteLine("Verification code sent successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
