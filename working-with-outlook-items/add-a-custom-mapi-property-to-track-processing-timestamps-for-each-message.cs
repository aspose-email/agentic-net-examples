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
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder message."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the message, add custom property, and save
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    string propertyName = "ProcessingTimestamp";
                    string timestamp = DateTime.UtcNow.ToString("o");
                    byte[] propertyValue = Encoding.Unicode.GetBytes(timestamp);

                    // Add custom Unicode property
                    message.AddCustomProperty(MapiPropertyType.PT_UNICODE, propertyValue, propertyName);

                    // Save the updated message
                    message.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MAPI message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
