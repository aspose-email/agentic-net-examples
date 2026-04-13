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
            // Define file paths
            string sourceMsgPath = "source.msg";
            string targetEmlPath = "target.eml";

            // Ensure the source MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(sourceMsgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(sourceMsgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    // Create a simple MapiMessage with minimal required fields
                    MapiMessage placeholderMessage = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "This is a placeholder message body."
                    );

                    // Save the placeholder as MSG
                    placeholderMessage.Save(sourceMsgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file into a MailMessage instance
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(sourceMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Use using to ensure MailMessage is disposed
            using (mailMessage)
            {
                // Configure save options to preserve embedded message formats
                EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                {
                    PreserveEmbeddedMessageFormat = true
                };

                // Save the MailMessage as EML with the specified options
                try
                {
                    mailMessage.Save(targetEmlPath, emlSaveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save EML file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
