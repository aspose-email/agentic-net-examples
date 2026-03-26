using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgFilePath = "sample.msg";
            string emlOutputPath = "output.eml";

            if (!File.Exists(msgFilePath))
            {
                // Create a minimal placeholder MSG file
                using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder message.", OutlookMessageFormat.Unicode))
                {
                    placeholder.Save(msgFilePath);
                }
            }

            using (MailMessage message = new MailMessage())
            {
                // Set basic properties
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Test with MSG AlternateView";
                message.Body = "This email contains an MSG alternate view.";

                // Load MSG file into an AlternateView
                using (FileStream msgStream = File.OpenRead(msgFilePath))
                {
                    // Read all bytes from the MSG file
                    MemoryStream memory = new MemoryStream();
                    msgStream.CopyTo(memory);
                    byte[] msgBytes = memory.ToArray();

                    // Define the content type for MSG format
                    ContentType msgContentType = new ContentType("application/vnd.ms-outlook");

                    // Create an AlternateView from the MSG bytes (encoded as Base64 string)
                    AlternateView msgView = AlternateView.CreateAlternateViewFromString(Convert.ToBase64String(msgBytes), msgContentType);

                    // Add the AlternateView to the message
                    message.AlternateViews.Add(msgView);
                }

                // Save the email to an EML file
                message.Save(emlOutputPath, SaveOptions.DefaultEml);
            }

            Console.WriteLine("Email with MSG alternate view saved to " + emlOutputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}