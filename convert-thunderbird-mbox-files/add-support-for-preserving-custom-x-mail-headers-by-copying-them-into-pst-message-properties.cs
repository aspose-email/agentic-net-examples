using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure PST file exists; create minimal PST if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode).Dispose();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open PST and add a message with custom X‑mail headers
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Get the Inbox folder (creates it if it does not exist)
                    FolderInfo inbox = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                    // Create a MailMessage and add custom X‑mail headers
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = "sender@example.com";
                    mailMessage.To = "recipient@example.com";
                    mailMessage.Subject = "Test Message with Custom Headers";
                    mailMessage.Body = "This is a test email.";
                    mailMessage.Headers.Add("X-Custom-Header1", "Value1");
                    mailMessage.Headers.Add("X-Custom-Header2", "Value2");

                    // Convert to MapiMessage
                    MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                    // Copy custom X‑mail headers into MAPI custom properties
                    foreach (string headerKey in mailMessage.Headers.Keys)
                    {
                        if (headerKey.StartsWith("X-", StringComparison.OrdinalIgnoreCase))
                        {
                            string headerValue = mailMessage.Headers[headerKey];
                            byte[] valueBytes = Encoding.Unicode.GetBytes(headerValue);
                            mapiMessage.AddCustomProperty(MapiPropertyType.PT_UNICODE, valueBytes, headerKey);
                        }
                    }

                    // Add the message to the PST folder
                    inbox.AddMessage(mapiMessage);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
