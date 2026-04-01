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
            // Paths to the source EML file and the target PST file
            string emlFilePath = "sample.eml";
            string pstFilePath = "output.pst";

            // Verify that the EML file exists
            if (!File.Exists(emlFilePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"EML file not found: {emlFilePath}");
                return;
            }

            // Ensure the PST file exists; create a new one if it does not
            if (!File.Exists(pstFilePath))
            {
                try
                {
                    PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {createEx.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Load the EML message
                using (MailMessage mailMessage = MailMessage.Load(emlFilePath))
                {
                    // Convert the MailMessage to a MapiMessage
                    using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                    {
                        // Get the Inbox folder (creates it if missing)
                        FolderInfo inboxFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                        // Append the message to the Inbox folder
                        inboxFolder.AddMessage(mapiMessage);
                    }
                }

                // Changes are saved when the PersonalStorage object is disposed
                // Optionally, you can explicitly save:
                // pst.SaveAs(pstFilePath, FileFormat.Pst);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
