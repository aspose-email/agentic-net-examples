using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "contact.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                if (msg.SupportedType == MapiItemType.Contact)
                {
                    using (MapiContact contact = (MapiContact)msg.ToMapiMessageItem())
                    {
                        // Display contact name
                        Console.WriteLine(contact.NameInfo.DisplayName);

                        // Display primary email address if available
                        if (contact.ElectronicAddresses != null && contact.ElectronicAddresses.Email1 != null)
                        {
                            Console.WriteLine(contact.ElectronicAddresses.Email1.EmailAddress);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The MSG file does not contain a contact.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
