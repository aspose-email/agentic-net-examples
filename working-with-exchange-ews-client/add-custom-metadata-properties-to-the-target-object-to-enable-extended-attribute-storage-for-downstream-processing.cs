using System;
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
            // Define the path for the MSG file.
            string msgPath = "sample.msg";

            // Ensure the directory exists.
            string directory = Path.GetDirectoryName(Path.GetFullPath(msgPath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // If the file does not exist, create a minimal placeholder MSG file.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create placeholder message: " + ex.Message);
                    return;
                }
            }

            // Load the existing MSG file, add a custom property, and save it back.
            try
            {
                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    // Add a custom Unicode property.
                    byte[] customData = Encoding.UTF8.GetBytes("CustomValue");
                    message.AddCustomProperty(MapiPropertyType.PT_UNICODE, customData, "X-Custom-Property");

                    // Save the updated message.
                    message.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error processing message: " + ex.Message);
                return;
            }

            Console.WriteLine("Custom property added successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}