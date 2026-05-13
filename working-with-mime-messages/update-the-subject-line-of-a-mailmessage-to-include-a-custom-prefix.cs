using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.eml";
            string customPrefix = "[Custom] ";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                using (FileStream placeholderStream = File.Create(inputPath))
                {
                    string minimalEml = "From: sender@example.com\r\nTo: recipient@example.com\r\nSubject: Original Subject\r\n\r\nBody.";
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(minimalEml);
                    placeholderStream.Write(bytes, 0, bytes.Length);
                }
            }

            // Load the email, update the subject, and save.
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                string originalSubject = message.Subject ?? string.Empty;
                message.Subject = customPrefix + originalSubject;
                message.Save(outputPath);
            }

            Console.WriteLine("Subject updated and saved to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
