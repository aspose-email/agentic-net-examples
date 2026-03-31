using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.eml";

            // Ensure the input file exists; if not, create a minimal placeholder email.
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
                    MailMessage placeholderMessage = new MailMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "This is a placeholder email generated because the input file was missing."
                    );
                    placeholderMessage.Save(inputPath);
                }
                catch (Exception placeholderEx)
                {
                    Console.Error.WriteLine("Failed to create placeholder email: " + placeholderEx.Message);
                    return;
                }
            }

            // Load the email, then save it again. The MailMessage class does not retain
            // extended MAPI properties, effectively removing unnecessary metadata.
            try
            {
                using (MailMessage emailMessage = MailMessage.Load(inputPath))
                {
                    emailMessage.Save(outputPath);
                    Console.WriteLine("Email saved without extended properties to: " + outputPath);
                }
            }
            catch (Exception loadSaveEx)
            {
                Console.Error.WriteLine("Error processing the email file: " + loadSaveEx.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
