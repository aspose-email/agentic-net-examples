using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Mime;

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
                using (MapiMessage placeholderMessage = new MapiMessage(
                    "placeholder@example.com",
                    "recipient@example.com",
                    "Placeholder",
                    "This is a placeholder MSG file.",
                    OutlookMessageFormat.Unicode))
                {
                    placeholderMessage.Save(msgFilePath);
                }
            }

            // Create a new MailMessage
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = "sender@example.com";
                mailMessage.To = "receiver@example.com";
                mailMessage.Subject = "Test Email with MSG AlternateView";

                // Create an AlternateView from the MSG file and add it to the message
                using (FileStream msgStream = File.OpenRead(msgFilePath))
                {
                    ContentType msgContentType = new ContentType("application/vnd.ms-outlook");
                    AlternateView msgAlternateView = new AlternateView(msgStream, msgContentType);
                    mailMessage.AlternateViews.Add(msgAlternateView);
                }

                // Optionally save the composed email to an EML file
                string emlOutputPath = "output.eml";
                mailMessage.Save(emlOutputPath, SaveOptions.DefaultEml);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return;
        }
    }
}
