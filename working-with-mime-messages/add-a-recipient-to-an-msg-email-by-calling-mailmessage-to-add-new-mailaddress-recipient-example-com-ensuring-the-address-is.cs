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

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(msgPath));
            if (!Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory: {ex.Message}");
                    return;
                }
            }

            // If the MSG file does not exist, create a minimal placeholder message
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = new MailAddress("sender@example.com");
                        placeholder.Subject = "Placeholder";
                        placeholder.Body = "This is a placeholder message.";
                        placeholder.Save(msgPath, SaveOptions.DefaultMsgUnicode);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the existing MSG, add a recipient, and save it back
            try
            {
                using (MailMessage mailMessage = MailMessage.Load(msgPath))
                {
                    mailMessage.To.Add(new MailAddress("recipient@example.com"));
                    mailMessage.Save(msgPath, SaveOptions.DefaultMsgUnicode);
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
