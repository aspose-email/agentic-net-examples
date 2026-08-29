using System;
using System.IO;
using Aspose.Email;
using System.Collections.Specialized;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";

            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            using (MailMessage mail = MailMessage.Load(msgPath))
            {
                // Retrieve specific header values
                string subject = mail.Headers["Subject"];
                string from = mail.Headers["From"];
                string to = mail.Headers["To"];

                Console.WriteLine($"Subject: {subject}");
                Console.WriteLine($"From: {from}");
                Console.WriteLine($"To: {to}");

                // Iterate all headers
                foreach (string key in mail.Headers.Keys)
                {
                    Console.WriteLine($"{key}: {mail.Headers[key]}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
