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
            // Path to the MSG file
            string msgPath = "message.msg";

            // Verify that the file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file into a MapiMessage instance
            using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
            {
                // Display basic properties of the loaded message
                Console.WriteLine($"Subject: {mapiMessage.Subject}");
                Console.WriteLine($"From: {mapiMessage.SenderName}");
                Console.WriteLine($"Body: {mapiMessage.Body}");

                // Convert the MapiMessage to a MailMessage for further processing
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                {
                    // Example processing: output the MailMessage subject
                    Console.WriteLine($"MailMessage Subject: {mailMessage.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            // Gracefully handle any unexpected errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
