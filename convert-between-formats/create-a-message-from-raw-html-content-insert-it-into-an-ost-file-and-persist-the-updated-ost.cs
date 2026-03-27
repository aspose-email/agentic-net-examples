using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths and HTML content
            string ostFilePath = "sample.ost";
            string htmlContent = "<html><body><h1>Hello, World!</h1><p>This is a test email.</p></body></html>";

            // Ensure the OST file exists; create a minimal one if it does not
            try
            {
                if (!File.Exists(ostFilePath))
                {
                    // Create a new OST (PST) file with Unicode format
                    PersonalStorage.Create(ostFilePath, FileFormatVersion.Unicode);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                return;
            }

            // Create a MailMessage with HTML body
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = "sender@example.com";
                mailMessage.To.Add("receiver@example.com");
                mailMessage.Subject = "Test HTML Message";
                mailMessage.HtmlBody = htmlContent;

                // Convert MailMessage to MapiMessage
                MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

                // Open the OST file and add the message to the Inbox folder
                try
                {
                    using (PersonalStorage personalStorage = PersonalStorage.FromFile(ostFilePath))
                    {
                        // Ensure the Inbox folder exists
                        FolderInfo inboxFolder;
                        try
                        {
                            inboxFolder = personalStorage.RootFolder.GetSubFolder("Inbox");
                        }
                        catch (Exception)
                        {
                            // Create Inbox folder if it does not exist
                            inboxFolder = personalStorage.RootFolder.AddSubFolder("Inbox");
                        }

                        // Add the MapiMessage to the folder
                        string entryId = inboxFolder.AddMessage(mapiMessage);
                        Console.WriteLine($"Message added with EntryId: {entryId}");
                    }
                }
                catch (Exception pstEx)
                {
                    Console.Error.WriteLine($"OST handling error: {pstEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
