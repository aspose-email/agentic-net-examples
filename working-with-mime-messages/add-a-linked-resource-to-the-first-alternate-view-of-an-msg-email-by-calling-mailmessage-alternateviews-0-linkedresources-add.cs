using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "input.msg";
            string outputMsgPath = "output.msg";
            string imagePath = "image.jpg";

            // Ensure the input MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                using (MailMessage placeholder = new MailMessage())
                {
                    placeholder.From = "placeholder@example.com";
                    placeholder.To = "recipient@example.com";
                    placeholder.Subject = "Placeholder";
                    placeholder.Body = "This is a placeholder message.";
                    placeholder.Save(inputMsgPath, SaveOptions.DefaultMsgUnicode);
                }
            }

            // Ensure the image file exists; create an empty placeholder if missing.
            if (!File.Exists(imagePath))
            {
                using (FileStream fs = File.Create(imagePath))
                {
                    // Write a minimal JPEG header to avoid zero‑byte file issues.
                    byte[] jpegHeader = new byte[] { 0xFF, 0xD8, 0xFF, 0xD9 };
                    fs.Write(jpegHeader, 0, jpegHeader.Length);
                }
            }

            // Load the MSG message.
            using (MailMessage message = MailMessage.Load(inputMsgPath))
            {
                // Ensure there is at least one alternate view.
                if (message.AlternateViews.Count == 0)
                {
                    AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                        "Placeholder plain text", null, "text/plain");
                    message.AlternateViews.Add(plainView);
                }

                // Add the linked resource to the first alternate view.
                AlternateView firstView = message.AlternateViews[0];
                firstView.LinkedResources.Add(new LinkedResource(imagePath));

                // Save the modified message.
                message.Save(outputMsgPath, SaveOptions.DefaultMsgUnicode);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
