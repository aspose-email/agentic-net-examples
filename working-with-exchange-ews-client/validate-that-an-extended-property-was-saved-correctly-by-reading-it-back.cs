using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "custom.msg";

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a new MAPI message and add a custom property
            MapiMessage message = new MapiMessage(
                "from@example.com",
                "to@example.com",
                "Sample Subject",
                "Sample body text.");

            byte[] customValue = Encoding.Unicode.GetBytes("CustomValue");
            message.AddCustomProperty(MapiPropertyType.PT_UNICODE, customValue, "CustomProp");

            // Save the message to a file
            try
            {
                message.Save(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                return;
            }

            // Load the message back and validate the custom property
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

                Console.Error.WriteLine("Message file does not exist.");
                return;
            }

            MapiMessage loadedMessage;
            try
            {
                loadedMessage = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load message: {ex.Message}");
                return;
            }

            // Retrieve custom properties
            MapiPropertyCollection customProperties = loadedMessage.GetCustomProperties();

            foreach (KeyValuePair<long, MapiProperty> entry in customProperties)
            {
                long propertyTag = entry.Key;
                string propertyName = MapiPropertyTag.GetPropertyName(propertyTag);
                string propertyValue = loadedMessage.GetPropertyString(propertyTag);
                Console.WriteLine($"Property: {propertyName} (Tag: 0x{propertyTag:X}) = {propertyValue}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
