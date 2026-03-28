using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MSG file containing the contact
                string msgFilePath = "contact.msg";

                // Verify that the file exists before attempting to load it
                if (!File.Exists(msgFilePath))
                {
                    Console.Error.WriteLine($"File not found: {msgFilePath}");
                    return;
                }

                // Load the MSG file as a MapiMessage (IDisposable)
                using (MapiMessage msg = MapiMessage.Load(msgFilePath))
                {
                    // Ensure the loaded message is a contact
                    if (msg.SupportedType == MapiItemType.Contact)
                    {
                        // Convert the MapiMessage to a MapiContact (IDisposable)
                        using (MapiContact contact = (MapiContact)msg.ToMapiMessageItem())
                        {
                            // Example processing: display contact name and primary email address
                            Console.WriteLine($"Display Name: {contact.NameInfo.DisplayName}");

                            // ElectronicAddresses may contain up to three email entries; use Email1 if available
                            if (contact.ElectronicAddresses != null && contact.ElectronicAddresses.Email1 != null)
                            {
                                Console.WriteLine($"Email: {contact.ElectronicAddresses.Email1.EmailAddress}");
                            }
                            else
                            {
                                Console.WriteLine("No email address found.");
                            }

                            // Additional processing of the contact can be performed here
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("The specified MSG file does not contain a contact.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Global exception handling to prevent unhandled crashes
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
