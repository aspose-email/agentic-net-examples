using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "template.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine("Message template file not found: " + msgPath);
                return;
            }

            try
            {
                using (MailMessage template = MailMessage.Load(msgPath))
                {
                    using (MailMessage message = new MailMessage())
                    {
                        // Populate fields from the template
                        message.From = template.From;
                        foreach (MailAddress address in template.To)
                        {
                            message.To.Add(address);
                        }
                        message.Subject = template.Subject;
                        message.Body = template.Body;

                        // Send the message via SMTP
                        try
                        {
                            using (SmtpClient client = new SmtpClient("smtp.example.com", "username", "password"))
                            {
                                client.Send(message);
                                Console.WriteLine("Message sent successfully.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine("SMTP error: " + ex.Message);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("File operation error: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
