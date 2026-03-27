using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the file that will be attached
            string attachmentPath = "sample.txt";

            // Ensure the attachment file exists; create a minimal placeholder if missing
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Placeholder content");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Error creating placeholder file: {ioEx.Message}");
                    return;
                }
            }

            // Construct the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "receiver@example.com";
                message.Subject = "Test email with attachment";
                message.Body = "Please see the attached file.";

                // Attach the file
                try
                {
                    message.Attachments.Add(new Attachment(attachmentPath));
                }
                catch (Exception attachEx)
                {
                    Console.Error.WriteLine($"Error adding attachment: {attachEx.Message}");
                    return;
                }

                // Save the message as an MSG file
                string msgPath = "output.msg";
                try
                {
                    message.Save(msgPath, SaveOptions.DefaultMsg);
                    Console.WriteLine($"Message saved to {msgPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Error saving MSG file: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
