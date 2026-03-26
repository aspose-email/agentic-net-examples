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

            // Ensure the MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder Subject", "Placeholder Body"))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and retrieve custom metadata fields.
            try
            {
                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    MapiPropertyCollection customProperties = message.GetCustomProperties();

                    foreach (KeyValuePair<long, MapiProperty> kvp in customProperties)
                    {
                        string propertyName = MapiPropertyTag.GetPropertyName(kvp.Key);
                        Console.WriteLine($"Custom Property: {propertyName}");
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