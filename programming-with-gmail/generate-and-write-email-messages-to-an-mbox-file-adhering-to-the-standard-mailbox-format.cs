using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the output MBOX file path.
            string mboxPath = "output.mbox";

            // Ensure the directory for the MBOX file exists.
            string directory = Path.GetDirectoryName(mboxPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create the MBOX writer with default save options.
            using (MboxrdStorageWriter writer = new MboxrdStorageWriter(mboxPath, new MboxSaveOptions()))
            {
                // First email message.
                using (MailMessage message1 = new MailMessage())
                {
                    message1.From = new MailAddress("alice@example.com", "Alice");
                    message1.To.Add(new MailAddress("bob@example.com", "Bob"));
                    message1.Subject = "Hello";
                    message1.Body = "This is a test email.";
                    writer.WriteMessage(message1);
                }

                // Second email message.
                using (MailMessage message2 = new MailMessage())
                {
                    message2.From = new MailAddress("carol@example.com", "Carol");
                    message2.To.Add(new MailAddress("dave@example.com", "Dave"));
                    message2.Subject = "Meeting";
                    message2.Body = "Let's schedule a meeting.";
                    writer.WriteMessage(message2);
                }
            }

            Console.WriteLine("MBOX file created successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
