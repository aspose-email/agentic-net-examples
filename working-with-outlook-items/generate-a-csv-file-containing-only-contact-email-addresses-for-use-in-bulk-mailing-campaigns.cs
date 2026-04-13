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
            string csvPath = "contacts.csv";

            // Guard PST file existence
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"PST file not found: {pstPath}");
                return;
            }

            // Ensure output directory exists
            string csvDirectory = Path.GetDirectoryName(csvPath);
            if (!string.IsNullOrEmpty(csvDirectory) && !Directory.Exists(csvDirectory))
            {
                Directory.CreateDirectory(csvDirectory);
            }

            // Open CSV writer
            using (StreamWriter csvWriter = new StreamWriter(csvPath, false))
            {
                // Write CSV header
                csvWriter.WriteLine("Email");

                // Load PST and process contacts
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Attempt to get the Contacts folder; fallback to Root if not present
                    FolderInfo contactsFolder;
                    try
                    {
                        contactsFolder = pst.RootFolder.GetSubFolder("Contacts");
                    }
                    catch (Exception)
                    {
                        contactsFolder = pst.RootFolder;
                    }

                    foreach (MessageInfo messageInfo in contactsFolder.EnumerateMessages())
                    {
                        using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                        {
                            // Process only contact items
                            if (mapiMessage.SupportedType == MapiItemType.Contact)
                            {
                                MapiContact contact = (MapiContact)mapiMessage.ToMapiMessageItem();
                                MapiContactElectronicAddressPropertySet electronic = contact.ElectronicAddresses;

                                if (electronic != null)
                                {
                                    if (electronic.Email1 != null && !string.IsNullOrEmpty(electronic.Email1.EmailAddress))
                                    {
                                        csvWriter.WriteLine(electronic.Email1.EmailAddress);
                                    }
                                    if (electronic.Email2 != null && !string.IsNullOrEmpty(electronic.Email2.EmailAddress))
                                    {
                                        csvWriter.WriteLine(electronic.Email2.EmailAddress);
                                    }
                                    if (electronic.Email3 != null && !string.IsNullOrEmpty(electronic.Email3.EmailAddress))
                                    {
                                        csvWriter.WriteLine(electronic.Email3.EmailAddress);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"CSV file created at: {csvPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
