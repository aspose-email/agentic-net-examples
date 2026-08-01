using Aspose.Email.PersonalInfo;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

namespace AsposeEmailContactReader
{
    // Author: Aspose.Email example - reads a contact from an Outlook MSG file.
    class Program
    {
        static void Main()
        {
            try
            {
                // Path to the MSG file containing the contact.
                string msgPath = "contact.msg";

                // Verify that the file exists before attempting to load it.
                if (!File.Exists(msgPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found: {msgPath}");
                    return;
                }

                // Load the MSG file as a MapiMessage.
                MapiMessage msg = MapiMessage.Load(msgPath);

                // Check whether the loaded message represents a contact.
                if (msg.SupportedType == MapiItemType.Contact)
                {
                    // Convert the MapiMessage to a MapiContact.
                    MapiContact contact = (MapiContact)msg.ToMapiMessageItem();

                    // Display basic contact information.
                    Console.WriteLine($"Display Name: {contact.NameInfo.DisplayName}");

                    // Email address may be stored in ElectronicAddresses.Email1.
                    if (contact.ElectronicAddresses != null && contact.ElectronicAddresses.Email1 != null)
                    {
                        Console.WriteLine($"Email: {contact.ElectronicAddresses.Email1.EmailAddress}");
                    }
                }
                else
                {
                    Console.WriteLine("The MSG file does not contain a contact.");
                }
            }
            catch (Exception ex)
            {
                // Output any unexpected errors without crashing the application.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
