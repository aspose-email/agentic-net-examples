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
            // Define output file path
            string outputPath = "customMessage.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a new MAPI message
            MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Custom Property Demo",
                "This is the body of the message.");

            // Define a unique name for the custom property
            string customPropertyName = "MyCustomStringProperty";

            // Encode the string value as Unicode bytes
            string customValue = "My custom string value";
            byte[] valueBytes = Encoding.Unicode.GetBytes(customValue);

            // Add the custom extended property (type string) using PT_UNICODE
            message.AddCustomProperty(MapiPropertyType.PT_UNICODE, valueBytes, customPropertyName);

            // Save the message to MSG format
            message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
