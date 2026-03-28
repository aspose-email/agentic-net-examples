using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the MSG file to be sent
            string msgPath = "email.msg";

            // Verify that the MSG file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            // Load the email message from the MSG file
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Initialize the SMTP client with placeholder server details
                using (SmtpClient client = new SmtpClient("smtp.example.com", 587, "username", "password", SecurityOptions.Auto))
                {
                    // Enable protocol logging and specify the log file name
                    client.EnableLogger = true;
                    client.LogFileName = "smtp.log";

                    // Send the loaded message
                    client.Send(message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
