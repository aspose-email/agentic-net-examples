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
                // Create a simple email message and save it as MSG.
                MailAddress fromAddress = new MailAddress("sender@example.com");
                MailAddress toAddress = new MailAddress("recipient@example.com");
                MailMessage placeholder = new MailMessage(fromAddress, toAddress);
                placeholder.Subject = "Sample Message";
                placeholder.Body = "This is a placeholder MSG file.";
                placeholder.Save(msgPath, SaveOptions.DefaultMsgUnicode);
                placeholder.Dispose();

                Console.WriteLine($"Created placeholder MSG file at \"{msgPath}\".");
            }

            // Load the MSG file and retrieve the sender's email address.
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                string senderEmail = message.From != null ? message.From.Address : string.Empty;
                Console.WriteLine($"Sender: {senderEmail}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
