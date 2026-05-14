using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Input PST file path
            string pstPath = "contacts.pst";
            // Output markdown file path
            string mdPath = "Contacts.md";

            // Guard input file existence
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Input PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
            string mdDirectory = Path.GetDirectoryName(mdPath);
            if (!string.IsNullOrEmpty(mdDirectory) && !Directory.Exists(mdDirectory))
            {
                try
                {
                    Directory.CreateDirectory(mdDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Prepare markdown lines
            List<string> markdownLines = new List<string>();
            markdownLines.Add("| Name | Email | Phone |");
            markdownLines.Add("|---|---|---|");

            // Open PST and process contacts
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the predefined Contacts folder
                FolderInfo contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                if (contactsFolder == null)
                {
                    Console.Error.WriteLine("Contacts folder not found in PST.");
                    return;
                }

                foreach (MessageInfo messageInfo in contactsFolder.EnumerateMessages())
                {
                    using (MapiMessage msg = pst.ExtractMessage(messageInfo))
                    {
                        // Verify the item is a contact
                        if (msg.SupportedType != MapiItemType.Contact)
                            continue;

                        // Convert to MapiContact
                        MapiContact contact = (MapiContact)msg.ToMapiMessageItem();

                        // Extract required fields
                        string name = contact.NameInfo?.DisplayName ?? string.Empty;
                        string email = contact.ElectronicAddresses?.Email1?.EmailAddress ?? string.Empty;
                        string phone = contact.Telephones?.PrimaryTelephoneNumber ?? string.Empty;

                        // Add a markdown row
                        markdownLines.Add($"| {EscapePipe(name)} | {EscapePipe(email)} | {EscapePipe(phone)} |");
                    }
                }
            }

            // Write markdown file
            try
            {
                File.WriteAllLines(mdPath, markdownLines);
                Console.WriteLine($"Markdown file created at: {mdPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write markdown file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper to escape pipe characters in markdown cells
    private static string EscapePipe(string input)
    {
        return input?.Replace("|", "\\|") ?? string.Empty;
    }
}
