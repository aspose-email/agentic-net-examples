using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define the output MSG file path
            string msgPath = Path.Combine(Environment.CurrentDirectory, "output.msg");

            // Ensure the target directory exists
            string directory = Path.GetDirectoryName(msgPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a MailMessage and set the sender address
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("sender@example.com");
            mailMessage.To.Add(new MailAddress("recipient@example.com"));
            mailMessage.Subject = "Sample Message";
            mailMessage.Body = "This is a test email.";

            // Convert to MapiMessage and save as MSG
            using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
            {
                mapiMessage.Save(msgPath);
            }

            Console.WriteLine($"Message saved to: {msgPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
