using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define PST file path
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST file at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the Inbox folder (create if it does not exist)
                FolderInfo inbox = pst.RootFolder.GetSubFolder("Inbox");
                if (inbox == null)
                {
                    inbox = pst.RootFolder.AddSubFolder("Inbox");
                }

                // Create a new MAPI message
                MapiMessage message = new MapiMessage("sender@example.com", "recipient@example.com", "Test Subject", "Test body");

                // Add a custom property (initial value)
                byte[] initialData = Encoding.UTF8.GetBytes("InitialValue");
                message.AddCustomProperty(MapiPropertyType.PT_UNICODE, initialData, "MyCustomProp");

                // Add the message to the Inbox folder
                string entryId = inbox.AddMessage(message);
                Console.WriteLine($"Message added to PST with EntryId: {entryId}");

                // Prepare updated custom property (new value)
                byte[] updatedData = Encoding.UTF8.GetBytes("UpdatedValue");
                // Use a tag in the named property range (0x8000) and PT_UNICODE type
                long customTag = 0x8000;
                MapiProperty updatedProperty = new MapiProperty("MyCustomProp", customTag, (long)MapiPropertyType.PT_UNICODE, updatedData);

                // Create a property collection containing the updated property
                MapiPropertyCollection updatedProperties = new MapiPropertyCollection();
                updatedProperties.Add(updatedProperty);

                // Update the message's custom property inside the PST
                pst.ChangeMessage(entryId, updatedProperties);
                Console.WriteLine("Custom property updated successfully within the PST.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
