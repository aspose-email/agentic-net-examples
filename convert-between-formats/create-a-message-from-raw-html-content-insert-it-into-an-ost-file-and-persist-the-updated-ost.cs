using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string ostPath = "sample.ost";

            // Verify that the OST file exists before proceeding.
            if (!File.Exists(ostPath))
            {
                Console.Error.WriteLine($"OST file not found at path: {ostPath}");
                return;
            }

            try
            {
                // Open the existing OST file.
                using (PersonalStorage personalStorage = PersonalStorage.FromFile(ostPath))
                {
                    // Use the root folder (or specify a subfolder as needed).
                    FolderInfo rootFolder = personalStorage.RootFolder;

                    // Create a MailMessage with HTML content.
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = "sender@example.com";
                    mailMessage.To.Add("recipient@example.com");
                    mailMessage.Subject = "Sample HTML Message";
                    mailMessage.HtmlBody = "<html><body><h1>Hello</h1><p>This is a test.</p></body></html>";

                    // Convert the MailMessage to a MapiMessage.
                    using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                    {
                        // Add the message to the OST folder.
                        string entryId = rootFolder.AddMessage(mapiMessage);
                        Console.WriteLine($"Message added with EntryId: {entryId}");
                    }
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"File operation failed: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
