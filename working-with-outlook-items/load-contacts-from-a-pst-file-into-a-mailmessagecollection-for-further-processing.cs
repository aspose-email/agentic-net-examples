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
            string pstPath = "contacts.pst";

            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the predefined Contacts folder
                FolderInfo contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);

                // Collection to hold the loaded contacts as MailMessage objects
                MailMessageCollection contacts = new MailMessageCollection();

                // Enumerate all messages in the Contacts folder
                foreach (MessageInfo messageInfo in contactsFolder.EnumerateMessages())
                {
                    using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                    {
                        // Convert MAPI contact message to MailMessage
                        MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions());

                        // Add to the collection for further processing
                        contacts.Add(mailMessage);
                    }
                }

                // Example processing: output the count of loaded contacts
                Console.WriteLine($"Loaded {contacts.Count} contacts from PST.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
