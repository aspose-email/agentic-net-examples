using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace PSTContactToVCard
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string pstPath = "contacts.pst";
                string outputDirectory = "vCards";

                // Verify PST file existence
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"PST file not found: {pstPath}");
                    return;
                }

                // Ensure output directory exists
                try
                {
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory '{outputDirectory}': {dirEx.Message}");
                    return;
                }

                // Open PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Get the predefined Contacts folder
                    FolderInfo contactsFolder = pst.GetPredefinedFolder(StandardIpmFolder.Contacts);

                    // Enumerate all messages in the Contacts folder
                    foreach (MessageInfo messageInfo in contactsFolder.EnumerateMessages())
                    {
                        try
                        {
                            // Extract the full MAPI message
                            using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                            {
                                // Process only contact items
                                if (mapiMessage.SupportedType == MapiItemType.Contact)
                                {
                                    // Convert to MapiContact
                                    MapiContact mapiContact = (MapiContact)mapiMessage.ToMapiMessageItem();

                                    // Build a safe file name for the vCard
                                    string baseFileName = string.IsNullOrEmpty(messageInfo.Subject) ? "Contact" : messageInfo.Subject;
                                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                                    {
                                        baseFileName = baseFileName.Replace(invalidChar, '_');
                                    }
                                    string vcardFilePath = Path.Combine(outputDirectory, $"{baseFileName}.vcf");

                                    // Save as vCard
                                    mapiContact.Save(vcardFilePath);
                                    Console.WriteLine($"Saved vCard: {vcardFilePath}");
                                }
                            }
                        }
                        catch (Exception msgEx)
                        {
                            Console.Error.WriteLine($"Failed to process message '{messageInfo.Subject}': {msgEx.Message}");
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
}
