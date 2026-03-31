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
            // Define paths
            string emlPath = "sample.eml";
            string pstPath = "sample.pst";

            // Ensure the input EML file exists; create a minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    string placeholderEml = "From: test@example.com\r\nTo: test@example.com\r\nSubject: Sample Email\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(emlPath, placeholderEml);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder EML: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for the PST file exists
            try
            {
                string pstDirectory = Path.GetDirectoryName(Path.GetFullPath(pstPath));
                if (!Directory.Exists(pstDirectory))
                {
                    Directory.CreateDirectory(pstDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error preparing PST directory: {ex.Message}");
                return;
            }

            // Load the EML file into a MailMessage
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading EML file: {ex.Message}");
                return;
            }

            // Convert MailMessage to MapiMessage
            MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage);

            // Create a new PST file (Unicode version)
            using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
            {
                // Get the default Inbox folder; create it if it does not exist
                FolderInfo inboxFolder;
                try
                {
                    inboxFolder = pst.RootFolder.GetSubFolder("Inbox");
                }
                catch
                {
                    // If the Inbox subfolder is missing, create it
                    inboxFolder = pst.RootFolder.AddSubFolder("Inbox");
                }

                // Add the MapiMessage to the Inbox folder
                try
                {
                    string entryId = inboxFolder.AddMessage(mapiMessage);
                    Console.WriteLine($"Message added to PST with EntryId: {entryId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error adding message to PST: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
