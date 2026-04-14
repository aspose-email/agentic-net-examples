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
            string outputPath = Path.Combine(Environment.CurrentDirectory, "HighVisibilityAlert.msg");

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new MAPI message
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "High Visibility Alert",
                "Please review the attached information as soon as possible."))
            {
                // Add a custom property for the reminder sound (encoded as Unicode bytes)
                byte[] soundBytes = Encoding.Unicode.GetBytes("alert.wav");
                message.AddCustomProperty(
                    MapiPropertyType.PT_UNICODE,
                    soundBytes,
                    "ReminderFileParameter");

                // Set a follow‑up flag with a custom request text
                FollowUpManager.SetFlag(message, "Action Required");

                // Save the message to disk
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved successfully to: {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save the message: {ioEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
