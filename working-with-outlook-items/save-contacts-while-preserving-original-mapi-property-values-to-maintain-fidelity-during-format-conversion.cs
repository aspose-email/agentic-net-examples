using Aspose.Email.PersonalInfo;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string inputMsgPath = "contact.msg";
            string outputVcfPath = "contact.vcf";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputVcfPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Guard input file existence; create a minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholderMsg = new MapiMessage())
                    {
                        placeholderMsg.Subject = "Placeholder Contact";
                        placeholderMsg.Body = "This is a placeholder contact message.";
                        // Set minimal contact properties via MapiContact conversion
                        using (MapiContact placeholderContact = new MapiContact())
                        {
                            placeholderContact.NameInfo.GivenName = "John";
                            placeholderContact.NameInfo.Surname = "Doe";
                            placeholderContact.ElectronicAddresses.Email1.EmailAddress = "john.doe@example.com";
                            // Save as MSG
                            placeholderContact.Save(inputMsgPath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            MapiMessage mapiMessage = null;
            try
            {
                mapiMessage = MapiMessage.Load(inputMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Ensure the loaded message is a contact
            if (mapiMessage.SupportedType != MapiItemType.Contact)
            {
                Console.Error.WriteLine("The loaded MSG file is not a contact.");
                mapiMessage.Dispose();
                return;
            }

            // Convert to MapiContact to preserve original MAPI properties
            MapiContact mapiContact = null;
            try
            {
                mapiContact = (MapiContact)mapiMessage.ToMapiMessageItem();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to convert to MapiContact: {ex.Message}");
                mapiMessage.Dispose();
                return;
            }

            // Save the contact to VCard preserving original MAPI properties
            try
            {
                // Use default save options which retain custom MAPI properties
                mapiContact.Save(outputVcfPath);
                Console.WriteLine($"Contact saved to VCard: {outputVcfPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save VCard: {ex.Message}");
            }
            finally
            {
                // Dispose resources
                mapiContact.Dispose();
                mapiMessage.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
