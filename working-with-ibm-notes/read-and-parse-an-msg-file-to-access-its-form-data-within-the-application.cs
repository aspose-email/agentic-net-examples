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

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgPath))
            {
                // Retrieve custom MAPI properties
                MapiPropertyCollection customProps = msg.GetCustomProperties();

                foreach (KeyValuePair<long, MapiProperty> kvp in customProps)
                {
                    MapiProperty prop = kvp.Value;

                    // Check for the form data property
                    if (prop.Descriptor != null && prop.Descriptor.Equals(KnownPropertyList.EmsAbFormData))
                    {
                        // Extract the form data as a string
                        string formData = prop.GetString();
                        Console.WriteLine("Form Data:");
                        Console.WriteLine(formData);
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
