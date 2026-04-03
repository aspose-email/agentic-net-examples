using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input TNEF file (e.g., winmail.dat) and output MSG file paths
            string inputTnefPath = "input.winmail.dat";
            string outputMsgPath = "output.msg";

            // Verify that the input TNEF file exists
            if (!File.Exists(inputTnefPath))
            {
                Console.Error.WriteLine($"Error: Input TNEF file not found – {inputTnefPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {outputDirectory}. {dirEx.Message}");
                    return;
                }
            }

            // Load the TNEF file into a MapiMessage
            using (MapiMessage message = MapiMessage.LoadFromTnef(inputTnefPath))
            {
                // Save the message as MSG, preserving all attachments and metadata
                try
                {
                    message.Save(outputMsgPath);
                    Console.WriteLine($"Successfully converted TNEF to MSG: {outputMsgPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Error: Failed to save MSG file – {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
