using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Network share folder where contacts will be saved
            string networkSharePath = @"\\networkshare\contacts";

            // Ensure the network share directory exists
            if (!Directory.Exists(networkSharePath))
            {
                try
                {
                    Directory.CreateDirectory(networkSharePath);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{networkSharePath}': {dirEx.Message}");
                    return;
                }
            }

            // Create a new MAPI contact
            using (MapiContact contact = new MapiContact())
            {
                // Set contact details using the correct properties
                contact.NameInfo.GivenName = "John";
                contact.NameInfo.Surname = "Doe";
                contact.ElectronicAddresses.Email1.EmailAddress = "john.doe@example.com";

                // Define the full file path for the vCard
                string filePath = Path.Combine(networkSharePath, "JohnDoe.vcf");

                // Save the contact to the network share
                try
                {
                    contact.Save(filePath);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save contact to '{filePath}': {saveEx.Message}");
                    return;
                }

                // Apply file permissions (grant Read/Write to Everyone)
                try
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    FileSecurity fileSecurity = fileInfo.GetAccessControl();
                    FileSystemAccessRule accessRule = new FileSystemAccessRule(
                        new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                        FileSystemRights.Read | FileSystemRights.Write,
                        AccessControlType.Allow);
                    fileSecurity.AddAccessRule(accessRule);
                    fileInfo.SetAccessControl(fileSecurity);
                }
                catch (Exception permEx)
                {
                    Console.Error.WriteLine($"Failed to set permissions on '{filePath}': {permEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
