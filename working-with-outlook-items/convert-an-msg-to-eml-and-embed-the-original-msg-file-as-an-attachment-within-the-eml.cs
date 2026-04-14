using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "input.msg";
            string outputEmlPath = "output.eml";

            // Verify input MSG file exists
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

                Console.Error.WriteLine($"Input file '{inputMsgPath}' not found.");
                return;
            }

            // Load the MSG file into a MailMessage instance
            using (MailMessage mailMessage = MailMessage.Load(inputMsgPath))
            {
                // Attach the original MSG file to the MailMessage
                try
                {
                    Attachment msgAttachment = new Attachment(inputMsgPath);
                    mailMessage.Attachments.Add(msgAttachment);
                }
                catch (Exception attachEx)
                {
                    Console.Error.WriteLine($"Failed to attach MSG file: {attachEx.Message}");
                    // Continue without attachment if needed
                }

                // Prepare EML save options, preserving embedded message format
                EmlSaveOptions emlOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                {
                    PreserveEmbeddedMessageFormat = true
                };

                // Save the MailMessage as an EML file
                try
                {
                    mailMessage.Save(outputEmlPath, emlOptions);
                    Console.WriteLine($"Converted MSG to EML and saved as '{outputEmlPath}'.");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save EML file: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
