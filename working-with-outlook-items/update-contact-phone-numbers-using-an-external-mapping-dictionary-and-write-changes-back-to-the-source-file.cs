using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;
using Aspose.Email.PersonalInfo;

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
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Placeholder PST created
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Mapping of old phone numbers to new phone numbers
            var phoneMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "123-4567", "555-0001" },
                { "987-6543", "555-0002" }
            };

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the Contacts folder (standard predefined folder)
                FolderInfo contactsFolder;
                try
                {
                    contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to get Contacts folder: {ex.Message}");
                    return;
                }

                // Iterate through each message in the Contacts folder
                foreach (MessageInfo messageInfo in contactsFolder.EnumerateMessages())
                {
                    MapiMessage msg;
                    try
                    {
                        msg = pst.ExtractMessage(messageInfo);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to extract message {messageInfo.EntryId}: {ex.Message}");
                        continue;
                    }

                    // Process only contact items
                    if (msg.SupportedType != MapiItemType.Contact)
                        continue;

                    // Convert to MapiContact
                    MapiContact contact = (MapiContact)msg.ToMapiMessageItem();

                    bool updated = false;

                    // Update phone numbers based on the mapping dictionary
                    var telSet = contact.Telephones;
                    var telProps = typeof(MapiContactTelephonePropertySet).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var prop in telProps)
                    {
                        if (prop.PropertyType != typeof(string) || !prop.CanRead || !prop.CanWrite)
                            continue;

                        string currentNumber = prop.GetValue(telSet) as string;
                        if (string.IsNullOrEmpty(currentNumber))
                            continue;

                        if (phoneMapping.TryGetValue(currentNumber, out string newNumber))
                        {
                            prop.SetValue(telSet, newNumber);
                            updated = true;
                        }
                    }

                    if (updated)
                    {
                        // Get the underlying MapiMessage after modifications
                        MapiMessage updatedMessage = contact.GetUnderlyingMessage();

                        // Update the existing message in the PST (no duplicate insert)
                        try
                        {
                            contactsFolder.UpdateMessage(messageInfo.EntryIdString, updatedMessage);
                            Console.WriteLine($"Updated contact: {contact.NameInfo?.DisplayName ?? "Unnamed"}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to update contact {contact.NameInfo?.DisplayName}: {ex.Message}");
                        }
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
