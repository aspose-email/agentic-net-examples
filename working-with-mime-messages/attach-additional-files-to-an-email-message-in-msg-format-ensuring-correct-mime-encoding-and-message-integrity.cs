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
            // Paths to the files that will be attached
            string attachmentPath1 = "attachment1.txt";
            string attachmentPath2 = "attachment2.pdf";

            // Verify that the attachment files exist
            if (!File.Exists(attachmentPath1) || !File.Exists(attachmentPath2))
            {
                Console.Error.WriteLine("Error: One or more attachment files were not found.");
                return;
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Test email with attachments";
                message.Body = "Please see the attached files.";

                // Add the first attachment
                using (Attachment attachment1 = new Attachment(attachmentPath1))
                {
                    message.Attachments.Add(attachment1);

                    // Add the second attachment
                    using (Attachment attachment2 = new Attachment(attachmentPath2))
                    {
                        message.Attachments.Add(attachment2);

                        // Save the message in MSG format with proper save options
                        MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                        message.Save("EmailWithAttachments.msg", saveOptions);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
