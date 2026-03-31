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
            string inputPath = "contact.msg";

            // Ensure the input file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "Placeholder Sender",
                        "sender@example.com",
                        "receiver@example.com",
                        "Placeholder Subject"))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and extract the contact.
            try
            {
                using (MapiMessage msg = MapiMessage.Load(inputPath))
                {
                    if (msg.SupportedType == MapiItemType.Contact)
                    {
                        MapiContact contact = (MapiContact)msg.ToMapiMessageItem();

                        // Example processing: display some contact fields.
                        Console.WriteLine($"Display Name: {contact.NameInfo.DisplayName}");
                        if (contact.ElectronicAddresses != null && contact.ElectronicAddresses.Email1 != null)
                        {
                            Console.WriteLine($"Email: {contact.ElectronicAddresses.Email1.EmailAddress}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The loaded MSG file is not a contact item.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
