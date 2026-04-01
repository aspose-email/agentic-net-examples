using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.eml";
            string outputPath = "sample.msg";

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

                try
                {
                    using (MailMessage placeholderMessage = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "This is a placeholder email body."))
                    {
                        placeholderMessage.Save(inputPath);
                    }
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ioEx.Message}");
                    return;
                }
            }

            // Load the email message and save it as MSG.
            try
            {
                using (MailMessage email = MailMessage.Load(inputPath))
                {
                    // Saving with a .msg extension uses the default MSG format.
                    email.Save(outputPath);
                }
            }
            catch (Exception msgEx)
            {
                Console.Error.WriteLine($"Error processing email message: {msgEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
