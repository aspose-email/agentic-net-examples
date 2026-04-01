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

                using (MailMessage placeholder = new MailMessage())
                {
                    placeholder.Subject = "Placeholder Subject";
                    placeholder.Save(msgPath, SaveOptions.DefaultMsgUnicode);
                }

                Console.WriteLine("Placeholder MSG file created at: " + msgPath);
                return;
            }

            // Load the MSG file and retrieve its subject.
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                string subject = message.Subject;
                Console.WriteLine("Subject: " + subject);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
