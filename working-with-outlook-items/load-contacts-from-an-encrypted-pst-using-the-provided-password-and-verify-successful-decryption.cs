using Aspose.Email.PersonalInfo;
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
            string pstPath = "EncryptedContacts.pst";
            string password = "SecretPassword";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Load the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                MessageStore store = pst.Store;

                // Verify password if the PST is protected
                if (store.IsPasswordProtected)
                {
                    if (!store.IsPasswordValid(password))
                    {
                        Console.Error.WriteLine("Invalid password for the PST file.");
                        return;
                    }
                }

                // Access the Contacts folder
                FolderInfo contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                if (contactsFolder == null)
                {
                    Console.Error.WriteLine("Contacts folder not found in the PST.");
                    return;
                }

                // Enumerate contact messages
                foreach (MessageInfo msgInfo in contactsFolder.EnumerateMessages())
                {
                    using (MapiMessage contactMsg = pst.ExtractMessage(msgInfo))
                    {
                        // Decrypt the message if it is encrypted
                        MapiMessage decrypted = contactMsg.IsEncrypted ? contactMsg.Decrypt() : contactMsg;

                        // Output basic contact information (Subject often holds the display name)
                        Console.WriteLine($"Contact Subject: {decrypted.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
