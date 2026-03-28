using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Define the path for the MSG file that will store the AMP email.
            string msgFilePath = "amp_message.msg";

            // Ensure the directory for the output file exists.
            string directory = Path.GetDirectoryName(msgFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create an AMP email, set basic properties and the AMP HTML body, then save as MSG.
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com", "Sender Name");
                ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                ampMessage.Subject = "Sample AMP Email";
                ampMessage.Body = "This is the plain‑text fallback body.";
                ampMessage.IsBodyHtml = true;
                ampMessage.AmpHtmlBody = "<amp-email><h1>Hello AMP!</h1></amp-email>";

                // Save the message as an Outlook MSG file.
                ampMessage.Save(msgFilePath);
            }

            // Verify that the file was created before attempting to load it.
            if (!File.Exists(msgFilePath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgFilePath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Load the saved MSG file back into a MailMessage instance.
            using (MailMessage loadedMessage = MailMessage.Load(msgFilePath))
            {
                Console.WriteLine($"Subject: {loadedMessage.Subject}");
                Console.WriteLine($"Plain Text Body: {loadedMessage.Body}");
                // The AMP HTML body is not directly exposed on MailMessage,
                // but it can be accessed if the message is cast to AmpMessage.
                if (loadedMessage is AmpMessage loadedAmp)
                {
                    Console.WriteLine($"AMP HTML Body: {loadedAmp.AmpHtmlBody}");
                }
                else
                {
                    Console.WriteLine("AMP content is not available in the loaded message.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
