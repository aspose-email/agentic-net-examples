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
            string pstPath = "contacts.pst";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the predefined Contacts folder
                FolderInfo contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                if (contactsFolder == null)
                {
                    Console.Error.WriteLine("Contacts folder not found in the PST.");
                    return;
                }

                // Enumerate all messages in the Contacts folder
                foreach (MessageInfo messageInfo in contactsFolder.EnumerateMessages())
                {
                    try
                    {
                        using (MapiMessage message = pst.ExtractMessage(messageInfo))
                        {
                            // Check if the item is a contact
                            if (string.Equals(message.MessageClass, "IPM.Contact", StringComparison.OrdinalIgnoreCase))
                            {
                                // Display basic contact information (Subject often contains the display name)
                                Console.WriteLine($"Contact: {message.Subject}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing message {messageInfo.Subject}: {ex.Message}");
                        // Continue with next message
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
