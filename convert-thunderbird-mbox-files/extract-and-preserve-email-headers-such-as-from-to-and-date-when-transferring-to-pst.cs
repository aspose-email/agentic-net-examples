using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Paths for PST and sample EML file
            string pstPath = "output.pst";
            string emlPath = "sample.eml";

            // Ensure the directory for PST exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Create a sample EML file if it does not exist
            if (!File.Exists(emlPath))
            {
                MailMessage sampleMessage = new MailMessage();
                sampleMessage.From = new MailAddress("sender@example.com");
                sampleMessage.To.Add(new MailAddress("recipient@example.com"));
                sampleMessage.Subject = "Test Email";
                sampleMessage.Body = "This is a test email.";
                sampleMessage.Date = DateTime.Now;
                sampleMessage.Save(emlPath);
            }

            // Load the MailMessage from the EML file
            MailMessage mailMessage;
            using (FileStream emlStream = File.OpenRead(emlPath))
            {
                mailMessage = MailMessage.Load(emlStream);
            }

            // Convert MailMessage to MapiMessage (preserves headers)
            MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

            // Create a new PST file (Unicode format)
            using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
            {
                // Get the predefined Inbox folder
                FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                // Add the message to the PST folder
                string entryId = inboxFolder.AddMessage(mapiMessage);
                Console.WriteLine($"Message added to PST with EntryId: {entryId}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
