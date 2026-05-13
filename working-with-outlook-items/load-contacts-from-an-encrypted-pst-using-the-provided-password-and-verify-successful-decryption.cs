using Aspose.Email.PersonalInfo;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "encrypted.pst";
            string password = "SecretPassword";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode)) { }
                    Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
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
                // Verify if the PST is password protected
                if (pst.Store.IsPasswordProtected)
                {
                    bool isValid = pst.Store.IsPasswordValid(password);
                    Console.WriteLine(isValid
                        ? "Password is valid. PST decrypted successfully."
                        : "Invalid password. Cannot decrypt PST.");
                    if (!isValid) return;
                }
                else
                {
                    Console.WriteLine("PST is not password protected.");
                }

                // Access the Contacts folder
                FolderInfo contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                Console.WriteLine($"Contacts folder contains {contactsFolder.ContentCount} items.");

                // Enumerate contacts (stored as MAPI messages)
                foreach (MessageInfo msgInfo in contactsFolder.EnumerateMessages())
                {
                    using (MapiMessage contactMessage = pst.ExtractMessage(msgInfo))
                    {
                        Console.WriteLine($"Contact Subject: {contactMessage.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
