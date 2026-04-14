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
            // Paths for configuration XML and messages folder
            string configPath = "config.xml";
            string messagesFolder = "Messages";

            // Ensure configuration file exists; create minimal placeholder if missing
            if (!File.Exists(configPath))
            {
                string placeholderXml = "<Configurations></Configurations>";
                File.WriteAllText(configPath, placeholderXml);
                Console.WriteLine($"Created placeholder configuration file at '{configPath}'.");
            }

            // Load configuration XML
            XDocument configDoc = XDocument.Load(configPath);
            // Example: each <Config Subject="New Subject"/> element
            var configElements = configDoc.Root?.Elements("Config");

            // Ensure messages folder exists
            if (!Directory.Exists(messagesFolder))
            {
                Directory.CreateDirectory(messagesFolder);
                Console.WriteLine($"Created messages folder at '{messagesFolder}'.");
            }

            // Get MSG files in the folder
            string[] msgFiles = Directory.GetFiles(messagesFolder, "*.msg");

            // If no MSG files, create a minimal placeholder message
            if (msgFiles.Length == 0)
            {
                string placeholderPath = Path.Combine(messagesFolder, "placeholder.msg");
                MapiMessage placeholderMessage = new MapiMessage(
                    "from@example.com",
                    "to@example.com",
                    "Placeholder Subject",
                    "This is a placeholder message body."
                );
                placeholderMessage.Save(placeholderPath);
                Console.WriteLine($"Created placeholder MSG file at '{placeholderPath}'.");
                msgFiles = new string[] { placeholderPath };
            }

            // Apply configurations to each message
            foreach (string msgFile in msgFiles)
            {
                // Load the MSG as a MapiMessage
                MapiMessage mapMessage = MapiMessage.Load(msgFile);
                // Apply each configuration element (if any)
                if (configElements != null)
                {
                    foreach (XElement config in configElements)
                    {
                        XAttribute subjectAttr = config.Attribute("Subject");
                        if (subjectAttr != null)
                        {
                            mapMessage.Subject = subjectAttr.Value;
                        }

                        XAttribute bodyAttr = config.Attribute("Body");
                        if (bodyAttr != null)
                        {
                            mapMessage.Body = bodyAttr.Value;
                        }

                        // Example of adding a custom MAPI property (Unicode string)
                        XAttribute customPropAttr = config.Attribute("CustomProperty");
                        if (customPropAttr != null)
                        {
                            string propName = "CustomProp";
                            byte[] propValue = System.Text.Encoding.Unicode.GetBytes(customPropAttr.Value);
                            mapMessage.AddCustomProperty(MapiPropertyType.PT_UNICODE, propValue, propName);
                        }
                    }
                }

                // Save the modified message back to the same file
                mapMessage.Save(msgFile);
                Console.WriteLine($"Processed message: {Path.GetFileName(msgFile)}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
