using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "amp_message.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            using (FileStream fileStream = File.OpenRead(msgPath))
            {
                using (AmpMessage ampMessage = new AmpMessage())
                {
                    ampMessage.Import(fileStream);

                    // Configure SMTP client (replace with actual server details)
                    using (SmtpClient smtpClient = new SmtpClient("smtp.example.com", 587, "username", "password"))
                    {
                        smtpClient.SecurityOptions = SecurityOptions.Auto;

                        try
                        {
                            smtpClient.Send(ampMessage);
                            Console.WriteLine("AMP email sent successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                        }
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
