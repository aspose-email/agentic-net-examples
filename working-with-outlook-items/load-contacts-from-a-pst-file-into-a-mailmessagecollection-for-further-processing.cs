using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace AsposeEmailPstContacts
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstFilePath = "contacts.pst";

                // Ensure the PST file exists; create a minimal placeholder if it does not.
                if (!File.Exists(pstFilePath))
                {
                    try
                    {
                        PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
                        Console.WriteLine($"Placeholder PST file created at '{pstFilePath}'.");
                    }
                    catch (Exception createEx)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder PST file: {createEx.Message}");
                        return;
                    }
                }

                // Load the PST file and extract contacts.
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    // Get the predefined Contacts folder.
                    FolderInfo contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                    if (contactsFolder == null)
                    {
                        Console.Error.WriteLine("Contacts folder not found in the PST file.");
                        return;
                    }

                    MailMessageCollection contactMessages = new MailMessageCollection();

                    // Enumerate all messages (contacts) in the Contacts folder.
                    foreach (MessageInfo messageInfo in contactsFolder.EnumerateMessages())
                    {
                        try
                        {
                            using (MapiMessage contactMessage = pst.ExtractMessage(messageInfo))
                            {
                                // Convert the MAPI contact message to a MailMessage.
                                MailMessage mailMessage = contactMessage.ToMailMessage(new MailConversionOptions());
                                contactMessages.Add(mailMessage);
                            }
                        }
                        catch (Exception msgEx)
                        {
                            Console.Error.WriteLine($"Failed to process a contact message: {msgEx.Message}");
                            // Continue processing remaining messages.
                        }
                    }

                    Console.WriteLine($"Total contacts loaded into MailMessageCollection: {contactMessages.Count}");
                    // Further processing of contactMessages can be performed here.
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
