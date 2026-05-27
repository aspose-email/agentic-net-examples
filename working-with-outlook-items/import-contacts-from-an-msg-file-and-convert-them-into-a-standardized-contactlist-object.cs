using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            const string msgPath = "contact.msg";

            // Ensure the MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.MessageClass = "IPM.Contact";
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file.
            MapiMessage msg;
            try
            {
                msg = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (msg)
            {
                // Verify that the MSG represents a contact.
                if (msg.SupportedType != MapiItemType.Contact)
                {
                    Console.WriteLine("The provided MSG file is not a contact item.");
                    return;
                }

                // Convert to MapiContact.
                MapiContact mapiContact = (MapiContact)msg.ToMapiMessageItem();

                // Map to Aspose.Email.PersonalInfo.Contact.
                Contact contact = new Contact
                {
                    DisplayName = mapiContact.NameInfo?.DisplayName
                };

                // Populate email addresses using EmailAddress objects.
                if (!string.IsNullOrEmpty(mapiContact.ElectronicAddresses?.Email1?.EmailAddress))
                {
                    contact.EmailAddresses.Add(new EmailAddress(mapiContact.ElectronicAddresses.Email1.EmailAddress));
                }
                if (!string.IsNullOrEmpty(mapiContact.ElectronicAddresses?.Email2?.EmailAddress))
                {
                    contact.EmailAddresses.Add(new EmailAddress(mapiContact.ElectronicAddresses.Email2.EmailAddress));
                }
                if (!string.IsNullOrEmpty(mapiContact.ElectronicAddresses?.Email3?.EmailAddress))
                {
                    contact.EmailAddresses.Add(new EmailAddress(mapiContact.ElectronicAddresses.Email3.EmailAddress));
                }

                // Create a standardized list of contacts.
                List<Contact> contactList = new List<Contact> { contact };

                // Output result.
                Console.WriteLine($"Imported {contactList.Count} contact(s).");
                Console.WriteLine($"Display Name: {contact.DisplayName}");
                foreach (EmailAddress email in contact.EmailAddresses)
                {
                    Console.WriteLine($"Email: {email.Address}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
