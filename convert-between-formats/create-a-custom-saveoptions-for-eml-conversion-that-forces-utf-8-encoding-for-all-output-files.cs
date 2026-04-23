using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string sourcePath = "source.eml";
            string targetPath = "output_utf8.eml";

            // Ensure the source file exists; create a minimal placeholder if missing
            if (!File.Exists(sourcePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(sourcePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                using (FileStream placeholderStream = new FileStream(sourcePath, FileMode.Create, FileAccess.Write))
                {
                    string placeholderContent = "From: sender@example.com\r\nTo: receiver@example.com\r\nSubject: Test\r\n\r\nThis is a test email.";
                    byte[] placeholderBytes = Encoding.UTF8.GetBytes(placeholderContent);
                    placeholderStream.Write(placeholderBytes, 0, placeholderBytes.Length);
                }
            }

            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(sourcePath))
            {
                // Force UTF-8 encoding for subject and body
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.BodyEncoding = Encoding.UTF8;

                // Create custom EML save options
                EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                {
                    CheckBodyContentEncoding = true
                };

                // Save the message with the custom options
                mailMessage.Save(targetPath, emlSaveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
