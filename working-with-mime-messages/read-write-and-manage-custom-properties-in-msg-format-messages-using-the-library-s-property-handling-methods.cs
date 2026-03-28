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
            string msgPath = "sample.msg";

            // Ensure the file exists; if not, create a minimal placeholder MSG.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("Placeholder Subject", "Placeholder Body", "sender@example.com", "recipient@example.com"))
                    {
                        placeholder.Save(msgPath);
                        Console.WriteLine($"Placeholder MSG created at '{msgPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file.
            try
            {
                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    // Add a custom property.
                    string customPropName = "MyCustomProperty";
                    string customPropValue = "CustomValue";
                    byte[] valueBytes = Encoding.UTF8.GetBytes(customPropValue);
                    message.AddCustomProperty(MapiPropertyType.PT_UNICODE, valueBytes, customPropName);

                    // Save the changes.
                    message.Save(msgPath);

                    // Retrieve and display all custom properties.
                    MapiPropertyCollection customProperties = message.GetCustomProperties();
                    foreach (KeyValuePair<long, MapiProperty> kvp in customProperties)
                    {
                        MapiProperty prop = kvp.Value;
                        string propName = prop.Name;
                        string propValue = prop.GetString();
                        Console.WriteLine($"{propName}: {propValue}");
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
