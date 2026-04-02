using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if it does not.
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

                using (MailMessage placeholder = new MailMessage("from@example.com", "to@example.com", "Placeholder", "Body"))
                {
                    placeholder.Save(msgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                }
            }

            // Load the MSG, add a custom header, display all headers, and save back.
            using (MailMessage mail = MailMessage.Load(msgPath))
            {
                mail.Headers.Add("X-My-Property", "12345");

                // Iterate headers using Keys as required.
                foreach (string key in mail.Headers.Keys)
                {
                    Console.WriteLine($"{key}: {mail.Headers[key]}");
                }

                mail.Save(msgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
