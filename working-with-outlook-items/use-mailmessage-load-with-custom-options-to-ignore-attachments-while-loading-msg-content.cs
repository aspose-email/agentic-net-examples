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
            string inputMsgPath = "sample.msg";
            string outputEmlPath = "output.eml";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Sample Subject",
                        "Sample body"))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Configure load options to ignore TNEF attachments (default behavior).
            MsgLoadOptions loadOptions = new MsgLoadOptions
            {
                PreserveTnefAttachments = false
            };

            // Load the MSG file with the custom options.
            using (MailMessage message = MailMessage.Load(inputMsgPath, loadOptions))
            {
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"Attachment count (should be 0 if ignored): {message.Attachments.Count}");

                // Save the loaded message as EML for demonstration.
                try
                {
                    message.Save(outputEmlPath);
                    Console.WriteLine($"Message saved to {outputEmlPath}");
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
