using Aspose.Email.PersonalInfo;
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
            // Input PST file containing contacts
            string pstPath = "contacts.pst";
            // Output CSV file with birthdays in ISO 8601 format
            string outputCsvPath = "contacts_birthdays.csv";

            // Ensure the PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage createdPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Create a Contacts folder so that the PST has a valid structure
                    createdPst.CreatePredefinedFolder("Contacts", StandardIpmFolder.Contacts);
                }
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputCsvPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the predefined Contacts folder
                FolderInfo contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);

                // Open the CSV writer
                using (StreamWriter writer = new StreamWriter(outputCsvPath))
                {
                    // Write CSV header
                    writer.WriteLine("DisplayName,Birthday");

                    // Enumerate all messages (contacts) in the folder
                    foreach (MessageInfo messageInfo in contactsFolder.EnumerateMessages())
                    {
                        // Extract the MAPI message
                        using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                        {
                            // Process only contact items
                            if (mapiMessage.SupportedType == MapiItemType.Contact)
                            {
                                // Convert to a MapiContact object
                                MapiContact mapiContact = (MapiContact)mapiMessage.ToMapiMessageItem();

                                // Retrieve display name
                                string displayName = mapiContact.NameInfo.DisplayName ?? string.Empty;

                                // Retrieve birthday; if not set, DateTime.MinValue is returned
                                DateTime birthday = mapiContact.Events.Birthday;
                                string birthdayString = birthday != DateTime.MinValue
                                    ? birthday.ToString("yyyy-MM-dd")
                                    : string.Empty;

                                // Write the contact information to CSV
                                writer.WriteLine($"{displayName},{birthdayString}");
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Contacts exported with birthdays in ISO 8601 format.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
