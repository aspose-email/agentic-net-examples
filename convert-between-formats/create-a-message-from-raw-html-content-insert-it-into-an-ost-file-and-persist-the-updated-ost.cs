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
            const string ostPath = "sample.ost";

            // Ensure the OST file exists; create a minimal one if missing
            if (!File.Exists(ostPath))
            {
                // Create a new OST (treated as PST with Unicode format)
                using (PersonalStorage pst = PersonalStorage.Create(ostPath, FileFormatVersion.Unicode))
                {
                    pst.RootFolder.AddSubFolder("Inbox");
                }
            }

            // Build a simple HTML email
            MailMessage mail = new MailMessage
            {
                From = new MailAddress("sender@example.com"),
                Subject = "Sample HTML Message",
                IsBodyHtml = true,
                HtmlBody = "<html><body><h1>Hello from Aspose.Email</h1></body></html>"
            };
            mail.To.Add(new MailAddress("recipient@example.com"));

            // Convert MailMessage to MapiMessage (required for OST insertion)
            MapiMessage mapiMessage = MapiMessage.FromMailMessage(mail);

            // Open the existing OST for modification
            using (PersonalStorage ost = PersonalStorage.FromFile(ostPath))
            {
                FolderInfo inbox;
                try
                {
                    inbox = ost.RootFolder.GetSubFolder("Inbox");
                }
                catch
                {
                    inbox = ost.RootFolder.AddSubFolder("Inbox");
                }

                // Add the message to the folder
                inbox.AddMessage(mapiMessage);
                // Changes are persisted when the storage is disposed
            }

            Console.WriteLine("Message added to OST successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
