using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "amp_message.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            try
            {
                using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
                {
                    MailConversionOptions conversionOptions = new MailConversionOptions();

                    using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                    {
                        try
                        {
                            using (SmtpClient smtpClient = new SmtpClient("smtp.example.com", 587, "username", "password", SecurityOptions.Auto))
                            {
                                smtpClient.Send(mailMessage);
                                Console.WriteLine("AMP email sent successfully.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error sending email: {ex.Message}");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
