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
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = new MailAddress("sender@example.com");
                        placeholder.To.Add(new MailAddress("initial@example.com"));
                        placeholder.Subject = "Placeholder Subject";
                        placeholder.Body = "This is a placeholder message.";
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the existing MSG file, modify recipients, and save it back.
            try
            {
                using (MailMessage message = MailMessage.Load(msgPath))
                {
                    // Add To recipients
                    message.To.Add(new MailAddress("to1@example.com"));
                    message.To.Add(new MailAddress("to2@example.com"));

                    // Add Cc recipients
                    message.CC.Add(new MailAddress("cc1@example.com"));
                    message.CC.Add(new MailAddress("cc2@example.com"));

                    // Add Bcc recipients
                    message.Bcc.Add(new MailAddress("bcc1@example.com"));
                    message.Bcc.Add(new MailAddress("bcc2@example.com"));

                    // Save the updated message back to the same file.
                    message.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
