using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

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
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                Console.WriteLine("Subject: " + msg.Subject);
                Console.WriteLine("From: " + msg.SenderName);
                Console.WriteLine("Body: " + msg.Body);

                // Process attachments
                string attachmentFolder = "Attachments";
                if (!Directory.Exists(attachmentFolder))
                {
                    Directory.CreateDirectory(attachmentFolder);
                }

                foreach (MapiAttachment att in msg.Attachments)
                {
                    Console.WriteLine("Attachment: " + att.FileName);
                    string attPath = Path.Combine(attachmentFolder, att.FileName);
                    using (FileStream fs = new FileStream(attPath, FileMode.Create, FileAccess.Write))
                    {
                        att.Save(fs);
                    }
                }

                // Convert to MailMessage for further processing if needed
                using (MailMessage mail = msg.ToMailMessage(new MailConversionOptions()))
                {
                    Console.WriteLine("MailMessage Subject: " + mail.Subject);
                    // Additional processing of MailMessage can be done here
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
