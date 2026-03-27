using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Mime;

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
                    using (MapiMessage placeholder = new MapiMessage(
                        "placeholder@example.com",
                        "recipient@example.com",
                        "Placeholder",
                        "This is a placeholder message.",
                        OutlookMessageFormat.Unicode))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }

                // Load the MSG file into a MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
                {
                    // Save the MapiMessage to a memory stream in MSG format
                    using (MemoryStream msgStream = new MemoryStream())
                    {
                        mapiMessage.Save(msgStream);
                        msgStream.Position = 0;

                        // Define the content type for MSG format
                        ContentType msgContentType = new ContentType("application/vnd.ms-outlook");

                        // Create an AlternateView from the MSG stream
                        AlternateView msgAlternateView = new AlternateView(msgStream, msgContentType);

                        // Create the main MailMessage
                        using (MailMessage mailMessage = new MailMessage())
                        {
                            mailMessage.From = "sender@example.com";
                            mailMessage.To = "receiver@example.com";
                            mailMessage.Subject = "Message with MSG Alternate View";
                            mailMessage.Body = "This email contains an MSG format alternate view.";

                            // Insert the MSG AlternateView into the collection
                            mailMessage.AlternateViews.Add(msgAlternateView);

                            // Save the composed email as an EML file
                            string emlPath = "output.eml";
                            mailMessage.Save(emlPath, SaveOptions.DefaultEml);
                            Console.WriteLine("Email saved to " + emlPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
