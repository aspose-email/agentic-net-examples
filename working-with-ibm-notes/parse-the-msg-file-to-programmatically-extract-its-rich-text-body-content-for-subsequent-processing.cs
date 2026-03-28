using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgParser
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string msgFilePath = "sample.msg";

                if (!File.Exists(msgFilePath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgFilePath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


                using (MapiMessage msg = MapiMessage.Load(msgFilePath))
                {
                    string richTextBody = msg.BodyRtf ?? string.Empty;
                    Console.WriteLine("Rich‑text body content:");
                    Console.WriteLine(richTextBody);
                    // TODO: Add further processing of richTextBody here.
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
