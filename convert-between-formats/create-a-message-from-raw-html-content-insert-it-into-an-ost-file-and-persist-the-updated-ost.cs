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
            // Paths for the OST file and optional placeholder creation
            string ostPath = "sample.ost";

            // Ensure the OST file exists; if not, create a new PST/OST placeholder
            if (!File.Exists(ostPath))
            {
                try
                {
                    // Create a new empty PST file (compatible with OST)
                    PersonalStorage.Create(ostPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create placeholder OST file: " + ex.Message);
                    return;
                }
            }

            // Raw HTML content for the email body
            string htmlContent = "<html><body><h1>Hello, World!</h1><p>This is a test email.</p></body></html>";

            // Build the MailMessage with HTML body
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = "sender@example.com";
            mailMessage.To = "receiver@example.com";
            mailMessage.Subject = "Test HTML Email";
            mailMessage.HtmlBody = htmlContent;

            // Convert MailMessage to MapiMessage for insertion into OST
            MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

            // Open the OST file for read/write operations
            using (PersonalStorage personalStorage = PersonalStorage.FromFile(ostPath))
            {
                // Access the root folder of the OST
                FolderInfo rootFolder = personalStorage.RootFolder;

                // Create (or retrieve) a subfolder named "MyFolder"
                FolderInfo targetFolder;
                try
                {
                    targetFolder = rootFolder.AddSubFolder("MyFolder");
                }
                catch (Exception)
                {
                    // If the folder already exists, retrieve it
                    targetFolder = rootFolder.GetSubFolder("MyFolder");
                }

                // Add the MAPI message to the target folder
                try
                {
                    string entryId = targetFolder.AddMessage(mapiMessage);
                    Console.WriteLine("Message added successfully. EntryId: " + entryId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to add message to OST: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}