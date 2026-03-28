using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgReader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string msgPath = "message.msg";

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
                    Console.WriteLine($"Subject: {msg.Subject}");
                    Console.WriteLine($"From: {msg.SenderName} <{msg.SenderEmailAddress}>");
                    Console.WriteLine($"Sent: {msg.ClientSubmitTime}");
                    Console.WriteLine($"Body: {msg.Body}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
