using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace AsposeEmailDragDropExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Path to the Outlook item (e.g., .msg file) that would be dragged onto the application.
                string outlookItemPath = "sample.msg";

                // Ensure the file exists; if not, create a minimal placeholder .msg file.
                if (!File.Exists(outlookItemPath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(outlookItemPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    try
                    {
                        MailMessage placeholder = new MailMessage();
                        placeholder.From = new MailAddress("sender@example.com");
                        placeholder.To = new MailAddressCollection();
                        placeholder.To.Add(new MailAddress("recipient@example.com"));
                        placeholder.Subject = "Placeholder Message";
                        placeholder.Body = "This is a placeholder Outlook message.";

                        // Save as an Outlook .msg file using Unicode format.
                        placeholder.Save(outlookItemPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode));
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder .msg file: {ex.Message}");
                        return;
                    }
                }

                // Simulate handling of a dragged Outlook item.
                // In a real UI DragEnter event, you would set e.Effect = DragDropEffects.Copy.
                // Here we simply output what would happen.
                Console.WriteLine("DragEnter event simulated: setting effect to Copy for Outlook items.");

                // Load the Outlook item to demonstrate core Aspose.Email processing.
                try
                {
                    using (MailMessage message = MailMessage.Load(outlookItemPath))
                    {
                        Console.WriteLine($"Loaded message subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"To: {string.Join(", ", message.To)}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load Outlook item: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
