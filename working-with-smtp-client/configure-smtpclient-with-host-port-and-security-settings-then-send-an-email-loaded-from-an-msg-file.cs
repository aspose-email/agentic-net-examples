using Aspose.Email.Clients;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            const string host = "smtp.example.com";
            const int port = 587;
            const string username = "user@example.com";
            const string password = "password123";
            const string msgPath = "email.msg";

            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Error: Message file '{msgPath}' not found.");
                return;
            }

            MapiMessage mapMsg;
            try
            {
                mapMsg = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            MailMessage mail;
            try
            {
                mail = mapMsg.ToMailMessage(new MailConversionOptions());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error converting MSG to MailMessage: {ex.Message}");
                return;
            }

            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    client.Send(mail);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
