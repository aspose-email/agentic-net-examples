using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Generate a simple 6‑digit OTP
            Random randomGenerator = new Random();
            int otpNumber = randomGenerator.Next(100000, 1000000);
            string otpCode = otpNumber.ToString();

            // Prepare the email message containing the OTP
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "recipient@example.com";
                message.Subject = "Your OTP Code";
                message.Body = "Your OTP is: " + otpCode;

                // Send the email using SmtpClient (placeholder credentials)
                try
                {
                    using (SmtpClient client = new SmtpClient("smtp.example.com", 587, "username", "password", SecurityOptions.Auto))
                    {
                        client.Send(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error sending OTP email: " + ex.Message);
                    return;
                }
            }

            // Prompt the user to enter the OTP received via SMS
            Console.Write("Enter the OTP you received: ");
            string userInput = Console.ReadLine();

            if (userInput == otpCode)
            {
                Console.WriteLine("OTP verification succeeded.");
            }
            else
            {
                Console.WriteLine("OTP verification failed.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}