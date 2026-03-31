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
            // Define paths
            string msgPath = "sample.msg";
            string outputPath = "output.eml";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body"))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Create the mail message
            using (MailMessage mail = new MailMessage())
            {
                mail.From = "sender@example.com";
                mail.To = "recipient@example.com";
                mail.Subject = "Message with MSG Alternate View";
                mail.Body = "This email contains an MSG format alternate view.";

                // Create AlternateView from the MSG file
                try
                {
                    using (AlternateView altView = new AlternateView(msgPath, "application/vnd.ms-outlook"))
                    {
                        mail.AlternateViews.Add(altView);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add AlternateView: {ex.Message}");
                    return;
                }

                // Save the composed message
                try
                {
                    mail.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MailMessage: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
