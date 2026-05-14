using System;
using System.IO;
using System.Xml.Linq;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "message.msg";
            string outputPath = "message_properties.xml";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder Subject";
                        placeholder.Body = "Placeholder body.";
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and export its MAPI properties to XML.
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    XDocument xmlDoc = new XDocument(new XElement("MapiProperties"));

                    foreach (MapiProperty property in message.Properties.Values)
                    {
                        // Retrieve the property value safely.
                        object propValue = null;
                        try
                        {
                            propValue = property.GetValue();
                        }
                        catch
                        {
                            // Ignore any exceptions while retrieving the value.
                        }

                        string valueString = propValue != null ? propValue.ToString() : string.Empty;

                        XElement propElement = new XElement("Property",
                            new XAttribute("Tag", property.Tag),
                            new XAttribute("Type", property.DataType),
                            new XAttribute("Value", valueString));

                        xmlDoc.Root.Add(propElement);
                    }

                    try
                    {
                        xmlDoc.Save(outputPath);
                        Console.WriteLine($"MAPI properties exported to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save XML file: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
