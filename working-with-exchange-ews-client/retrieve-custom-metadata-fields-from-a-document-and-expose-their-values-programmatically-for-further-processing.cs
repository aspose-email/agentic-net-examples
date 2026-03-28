using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder", "Body"))
                {
                    placeholder.Save(msgPath);
                }
                Console.Error.WriteLine($"Input file not found. Created placeholder at {msgPath}.");
                return;
            }

            // Load the Outlook message file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Retrieve custom MAPI properties
                MapiPropertyCollection customProps = msg.GetCustomProperties();

                if (customProps == null || customProps.Count == 0)
                {
                    Console.WriteLine("No custom properties found.");
                }
                else
                {
                    foreach (KeyValuePair<long, MapiProperty> kvp in customProps)
                    {
                        // Access the property value using appropriate getter; here we use GetPropertyString as an example
                        string value = msg.GetPropertyString(kvp.Key);
                        Console.WriteLine($"Property Tag: 0x{kvp.Key:X}, Value: {value}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
