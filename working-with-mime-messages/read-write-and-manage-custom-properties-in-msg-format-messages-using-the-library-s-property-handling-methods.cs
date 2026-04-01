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
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                using (MapiMessage placeholder = new MapiMessage("sender@example.com", "receiver@example.com", "Placeholder", "This is a placeholder message."))
                {
                    placeholder.Save(inputPath);
                    Console.WriteLine($"Placeholder MSG created at {inputPath}");
                }
            }

            // Load the MSG file.
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG: {ex.Message}");
                return;
            }

            using (message)
            {
                // Add a custom property (Unicode string "CustomValue").
                byte[] valueBytes = System.Text.Encoding.Unicode.GetBytes("CustomValue");
                message.AddCustomProperty(MapiPropertyType.PT_UNICODE, valueBytes, "CustomProp");

                // Save the modified message.
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Modified MSG saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG: {ex.Message}");
                    return;
                }

                // Retrieve and display custom properties.
                MapiPropertyCollection customProps = message.GetCustomProperties();
                Console.WriteLine("Custom properties:");
                foreach (MapiProperty prop in customProps.Values)
                {
                    string propName = prop.Name ?? "Unnamed";
                    string propValue = System.Text.Encoding.Unicode.GetString(prop.Data);
                    Console.WriteLine($"- {propName}: {propValue}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
