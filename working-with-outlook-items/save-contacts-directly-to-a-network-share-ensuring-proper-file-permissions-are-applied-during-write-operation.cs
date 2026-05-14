using Aspose.Email;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Define the network share folder and the contact file name
            string networkFolder = @"\\server\share\Contacts";
            string contactFile = Path.Combine(networkFolder, "JohnDoe.vcf");

            // Ensure the network folder exists
            if (!Directory.Exists(networkFolder))
            {
                try
                {
                    Directory.CreateDirectory(networkFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create network folder: {ex.Message}");
                    return;
                }
            }

            // Create a new contact and populate fields
            Contact contact = new Contact
            {
                GivenName = "John",
                Surname = "Doe"
            };
            contact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));
            contact.PhoneNumbers.Add(new PhoneNumber { Number = "123-456-7890", Category = PhoneNumberCategory.Company });

            // Save the contact to the network share as a vCard file
            try
            {
                contact.Save(contactFile);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save contact: {ex.Message}");
                return;
            }

            // Apply file permissions to the saved vCard file
            try
            {
                FileInfo fileInfo = new FileInfo(contactFile);
                FileSecurity security = fileInfo.GetAccessControl();

                // Grant read/write access to Everyone (adjust as needed)
                var rule = new FileSystemAccessRule(
                    new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                    FileSystemRights.Read | FileSystemRights.Write,
                    AccessControlType.Allow);

                security.AddAccessRule(rule);
                fileInfo.SetAccessControl(security);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to set file permissions: {ex.Message}");
                return;
            }

            Console.WriteLine("Contact saved successfully to the network share.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
