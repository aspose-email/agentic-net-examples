using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder";
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the existing MSG file.
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load message: {ex.Message}");
                return;
            }

            using (message)
            {
                // Display existing custom properties (if any).
                try
                {
                    MapiPropertyCollection customProps = message.GetCustomProperties();
                    Console.WriteLine("Existing custom properties:");
                    foreach (KeyValuePair<long, MapiProperty> kvp in customProps)
                    {
                        long tag = kvp.Key;
                        Console.WriteLine($"  Tag: 0x{tag:X}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error reading custom properties: {ex.Message}");
                }

                // Add a new custom property.
                try
                {
                    string customValue = "CustomValue";
                    byte[] valueBytes = Encoding.UTF8.GetBytes(customValue);
                    message.AddCustomProperty(MapiPropertyType.PT_UNICODE, valueBytes, "X-Custom-Prop");
                    Console.WriteLine("Added custom property 'X-Custom-Prop'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error adding custom property: {ex.Message}");
                }

                // Save the modified message.
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}