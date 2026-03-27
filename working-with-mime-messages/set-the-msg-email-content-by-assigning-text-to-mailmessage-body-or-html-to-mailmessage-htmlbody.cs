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
            // Path where the MSG file will be saved
            string outputPath = "output.msg";

            // Ensure the target directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a MailMessage and set its plain‑text body
            using (MailMessage mail = new MailMessage())
            {
                mail.From = "sender@example.com";
                mail.To = "recipient@example.com";
                mail.Subject = "Sample Subject";
                mail.Body = "This is the plain text body of the message.";

                // Convert the MailMessage to a MapiMessage (MSG format)
                using (MapiMessage mapi = MapiMessage.FromMailMessage(mail))
                {
                    // Save the MSG file
                    mapi.Save(outputPath);
                }
            }

            Console.WriteLine($"Message saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
