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
            string messagePath = "message.msg";

            // Ensure the file exists; create a minimal placeholder if it does not.
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body"))
                    {
                        placeholder.Save(messagePath);
                    }

                    Console.WriteLine("Placeholder message created at: " + messagePath);
                    return;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create placeholder message: " + ex.Message);
                    return;
                }
            }

            // Load the existing message and remove the unwanted extended property.
            try
            {
                using (MapiMessage message = MapiMessage.Load(messagePath))
                {
                    // Example: remove a custom property named "MyCustomProp" if it exists.
                    MapiPropertyCollection customProps = message.GetCustomProperties();

                    foreach (var entry in customProps)
                    {
                        // entry is KeyValuePair<long, MapiProperty>
                        MapiProperty prop = entry.Value;
                        if (prop.Name == "MyCustomProp")
                        {
                            // Remove the property by its tag.
                            message.RemoveProperty(prop.Tag);
                            Console.WriteLine("Removed custom property: " + prop.Name);
                            break;
                        }
                    }

                    // Alternatively, remove a known extended property (e.g., DeleteAfterSubmit) directly.
                    // long deleteAfterSubmitTag = KnownPropertyList.DeleteAfterSubmit.Tag;
                    // message.RemoveProperty(deleteAfterSubmitTag);

                    // Save the updated message back to the same file.
                    message.Save(messagePath);
                    Console.WriteLine("Message saved after property removal.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error processing the message: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
