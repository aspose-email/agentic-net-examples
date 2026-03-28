using System;
using System.IO;
using Aspose.Email;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Path to the MSG file that will be used as an alternate view
                string msgFilePath = "sample.msg";

                // Ensure the MSG file exists; create a minimal placeholder if it does not
                if (!File.Exists(msgFilePath))
                {
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = "placeholder@example.com";
                        placeholder.To.Add("placeholder@example.com");
                        placeholder.Subject = "Placeholder";
                        placeholder.Body = "Placeholder body";
                        placeholder.Save(msgFilePath, SaveOptions.DefaultMsgUnicode);
                    }
                }

                // Create the main mail message
                using (MailMessage message = new MailMessage())
                {
                    message.From = "sender@example.com";
                    message.To.Add("receiver@example.com");
                    message.Subject = "Message with MSG AlternateView";
                    message.Body = "This email contains an MSG format alternate view.";

                    // Create an AlternateView from the MSG file (using the appropriate media type)
                    AlternateView msgAlternateView = new AlternateView(msgFilePath, "application/vnd.ms-outlook");

                    // Add the alternate view to the message
                    message.AlternateViews.Add(msgAlternateView);

                    // Save the composed message
                    string outputPath = "output.msg";
                    message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
