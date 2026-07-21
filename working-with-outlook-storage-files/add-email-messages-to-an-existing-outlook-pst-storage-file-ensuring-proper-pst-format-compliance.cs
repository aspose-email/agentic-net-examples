using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the PST file
            string pstPath = "storage.pst";

            // Ensure the PST file exists; create a new one if it does not.
            if (!File.Exists(pstPath))
            {
                // Create a new PST file with Unicode format.
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
            }

            // Open the existing PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // -------------------------------------------------
                // Add a message to the root folder of the PST file.
                // -------------------------------------------------
                MailMessage message1 = new MailMessage();
                message1.From = new MailAddress("alice@example.com");
                message1.To.Add(new MailAddress("bob@example.com"));
                message1.Subject = "Test Message 1";
                message1.Body = "This is a test email added to PST.";

                // Add the message to the root folder.
                pst.RootFolder.AddMessage(MapiMessage.FromMailMessage(message1));

                // -------------------------------------------------
                // Add a message to a subfolder (create if needed).
                // -------------------------------------------------
                FolderInfo subFolder;
                try
                {
                    // Attempt to create the subfolder.
                    subFolder = pst.RootFolder.AddSubFolder("Samples");
                }
                catch (Exception)
                {
                    // If the folder already exists, retrieve it.
                    subFolder = pst.RootFolder.GetSubFolder("Samples");
                }

                MailMessage message2 = new MailMessage();
                message2.From = new MailAddress("carol@example.com");
                message2.To.Add(new MailAddress("dave@example.com"));
                message2.Subject = "Test Message 2";
                message2.Body = "Another test email in a subfolder.";

                // Add the second message to the subfolder.
                subFolder.AddMessage(MapiMessage.FromMailMessage(message2));

                Console.WriteLine("Messages added successfully to the PST file.");
            }
        }
        catch (Exception ex)
        {
            // Log any errors without crashing the application.
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
